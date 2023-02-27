using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using External.MeshWelder;
using MarchingCubesProject;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace GemCutting
{
    [Serializable]
    public class GemStoneSerialization
    {
        [SerializeField] private List<Material> m_materials;
        [SerializeField] private Vector3Int m_size;
        [SerializeField] private int m_seed;
        [SerializeField] private float m_purityBias;
        [SerializeField] private VoxelGrid m_voxels;
        
        [NonSerialized] private bool m_valid;

        public List<Material> Materials => m_materials;
        public Vector3Int Size => m_size;
        public int Seed => m_seed;
        public float PurityBias => m_purityBias;
        public VoxelGrid Voxels => m_voxels;
        public bool Valid => m_valid;

        public GemStoneSerialization(GemStone gemstone)
        {
            // TODO: serialize base materials by guid?
            m_materials = gemstone.BaseMaterials;
            m_size = gemstone.Size;
            m_seed = gemstone.Seed;
            m_purityBias = gemstone.PurityBias;
            m_voxels = gemstone.Voxels;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public static GemStoneSerialization FromJson(string json)
        {
            // TODO: Handle parse errors. GemStoneSerialization as out argument?
            GemStoneSerialization deserialized = JsonUtility.FromJson<GemStoneSerialization>(json);
            
            // Validate incoming json
            deserialized.m_valid = true;
            
            // Validation isn't trivial (possible to cheat by manually adding removed voxels back in,
            // but leave removed interior voxels, and voxels that were never filled in the first place, empty)
            VoxelGrid validationVoxels = new(deserialized.m_size);
            validationVoxels.GenerateFromSeed(deserialized.m_seed, true, deserialized.m_purityBias);
            // Get the convex hull of the serialized voxel grid's positive values.
            // Within this hull, the serialized grid should match the validation grid exactly.
            // Outside of the hull, the serialized grid should be empty.
            // This indicates the serialized grid could conceivably have been cut from the validation grid.
            VoxelGrid convexHullVoxels = new(deserialized.m_size, 1.0f);
            IEnumerable<Vector3> validationNormals = new List<Vector3>()
                .Concat(GemTable.cuttingPlaneHorizontalNormals)
                .Concat(GemTable.cuttingPlaneSlantedNormals)
                .Concat(GemTable.validationAdditionalAngles);
            foreach (Vector3 planeNormal in validationNormals)
            {
                Vector3 planePosition = deserialized.m_voxels.GetCoordinateFurthestAlongNormal(planeNormal);
                convexHullVoxels.SetValuesAtAndBeyondPlane(
                    planePosition + (planeNormal * (VoxelGrid.Tolerance * 2)), // Nudge plane to ensure shell of hull is considered
                    planeNormal, 
                    0.0f);
            }
            // At this point, interestingVoxels should have values of 1 inside the convex hull and 0 outside
            for (int idx = 0; idx < convexHullVoxels.m_values.Length; idx++)
            {
                // Inside convex hull; values must match exactly
                if (Math.Abs(convexHullVoxels.m_values[idx] - 1.0f) < VoxelGrid.Tolerance)
                {
                    if (Math.Abs(deserialized.m_voxels.m_values[idx] - validationVoxels.m_values[idx]) > VoxelGrid.Tolerance)
                    {
                        deserialized.m_valid = false;
                        break;
                    }
                }
                // Outside convex hull; serialized value must be a void!
                else if (Math.Abs(convexHullVoxels.m_values[idx] - 0.0f) < VoxelGrid.Tolerance)
                {
                    if (Math.Abs(deserialized.m_voxels.m_values[idx] - validationVoxels.m_defaultValue) > VoxelGrid.Tolerance)
                    {
                        deserialized.m_valid = false;
                        break;
                    }
                }
            }
            
            return deserialized;
        }
    }
    
    /// <summary>
    /// Class representing a voxel grid which can be visualized as a gemstone.
    /// </summary>
    public class GemStone : MonoBehaviour 
    {
        private static readonly int CutPlanePositionProperty = Shader.PropertyToID("_CutPlanePosition");
        private static readonly int CutPlaneNormalProperty = Shader.PropertyToID("_CutPlaneNormal");
        private static readonly int UVOffsetProperty = Shader.PropertyToID("_UVOffset");

        [Header("Debugging")]
        [SerializeField] private bool m_showGizmoPlanes = true;
        [SerializeField] private bool m_showVoxelValues;
        [SerializeField] private bool m_useMaterialInstances = true; // Handy to use materials directly for iteration
        [Header("Generation Params")]
        [SerializeField] private List<Material> m_materials = new();
        [SerializeField] private VoxelsSource m_voxelsSource = VoxelsSource.Custom;
        [SerializeField] private GemCatalogType m_gemCatalogType = GemCatalogType.AidennMask;
        [SerializeField] private Vector3Int m_size = new(7, 7, 7);
        [SerializeField] private int m_seed;
        [SerializeField] private float m_purityBias = .05f;  // 0 = ~50% voids, 50% solids. Higher = fewer voids.
    
        private VoxelGrid m_voxels;
        private readonly List<GameObject> m_meshes = new();
        private readonly List<Material> m_materialInstances = new();
        // Material properties derived from seed
        private Vector4 m_uvOffset;

        public List<Material> BaseMaterials => m_materials;
        private List<Material> UsedMaterials => m_materialInstances.Any() ? m_materialInstances : m_materials;
        public float PurityBias => m_purityBias;
        public VoxelGrid Voxels => m_voxels;
        
        public Vector3Int Size => m_voxels?.m_extents ?? m_size;

        public int Seed
        {
            get => m_seed;
            set => m_seed = value;
        }

        public VoxelsSource VoxelsSource
        {
            get => m_voxelsSource;
            set => m_voxelsSource = value;
        }

        public GemCatalogType GemCatalogType
        {
            get => m_gemCatalogType;
            set => m_gemCatalogType = value;
        }
        
        #region Serialization
        public string Save()
        {
            // TODO: Save to local file or copy to clipboard
            GemStoneSerialization serializationObject = new(this);
            // TODO: Separate method
            GUIUtility.systemCopyBuffer = serializationObject.ToJson();
            return serializationObject.ToJson();
        }

        public bool Load()
        {
            // TODO: From clipboard
            string json = GUIUtility.systemCopyBuffer;
            
            // TODO: Respect validity of serializationObject, log/present to player if invalid
            GemStoneSerialization serializationObject = GemStoneSerialization.FromJson(json);
            if (!serializationObject.Valid)
            {
                return false;
            }
            // Construct gem from serializationObject
            m_voxelsSource = VoxelsSource.Custom; // It would be pretty silly to serialize other gem types.
            m_materials = serializationObject.Materials;
            m_size = serializationObject.Size;
            m_seed = serializationObject.Seed;
            m_purityBias = serializationObject.PurityBias;
            m_voxels = serializationObject.Voxels;
            
            Initialize();

            return true;
        }
        #endregion

        void Awake () {
            Initialize();
        }

        public void Initialize()
        {
            GenerateRandomMaterialValues();
            if (m_useMaterialInstances)
            {
                GenerateMaterialInstances();
            }
            UpdateVoxelsAndMesh();
        }

        private void GenerateMaterialInstances()
        {
            // Clear existing material instances
            foreach (GameObject mesh in m_meshes)
            {
                mesh.GetComponent<Renderer>().material = null;
            }
            foreach (Material materialInstance in m_materialInstances)
            {
                Destroy(materialInstance);
            }
            m_materialInstances.Clear();
            
            // Create materials
            foreach (Material material in m_materials)
            {
                Material materialInstance = new(material);
                // Apply seed-based properties
                materialInstance.SetVector(UVOffsetProperty, m_uvOffset);
                
                m_materialInstances.Add(materialInstance);
            }
            
        }

        private void GenerateRandomMaterialValues()
        {
            using (new HoldRandomStateScope(Seed))
            {
                m_uvOffset = new Vector4(Random.value, Random.value, Random.value, Random.value);
            }
        }

        private void UpdateVoxelsAndMesh() {
            if (m_voxels == null || m_voxels.m_extents != m_size)
            {
                m_voxels = new VoxelGrid(m_size);
            }

            switch (m_voxelsSource)
            {
                case VoxelsSource.Custom:
                {
                    // Just regenerate the meshes
                    UpdateMeshes();
                    break;
                }
                case VoxelsSource.Seed:
                {
                    // Generate voxels from seed, and meshes
                    m_voxels.GenerateFromSeed(m_seed, threshold: true, bias: m_purityBias);
                    UpdateMeshes();
                    break;
                }
                case VoxelsSource.Catalog:
                {
                    // Load catalog gem
                    // Getting a type from a different assembly is fun. GemTypes needs to be in the same assembly as the types themselves.
                    // Provide the fully qualified name to get the type!
                    Type gemClass = Type.GetType(typeof(GemCatalogType).Namespace + "." + m_gemCatalogType);
                    if (gemClass != null)
                    {
                        m_voxels = (VoxelGrid)Activator.CreateInstance(gemClass);
                        UpdateMeshes();
                    }

                    break;
                }
            }
        }

        // TODO: This hitches. Thread creation of new mesh, only destroy & show new mesh once done?
        // Alternatively, cache tris for each voxel and only regenerate those which have changed?
        private void UpdateMeshes()
        {
            if (m_voxels == null)
            {
                return;
            }

            // Nuke previous meshes
            foreach (GameObject visMesh in m_meshes)
            {
                Destroy(visMesh);
            }
            m_meshes.Clear();

            if (m_useMaterialInstances && !m_materialInstances.Any())
            {
                GenerateMaterialInstances();
            }

            // Pad the voxel grid to ensure we see edges.
            VoxelGrid visualGrid = new(m_voxels.m_extents + new Vector3Int(3,3,3), m_voxels.m_defaultValue);
            visualGrid.CopyFrom(m_voxels, 2, 2, 2);

            //Profiling.Profiler.BeginSample("Gem - Marching Cubes");
            Marching marching = new MarchingCubes(0.0f);
            List<Vector3> verts = new();
            List<int> indices = new();
            marching.Generate(visualGrid.m_values, visualGrid.m_extents.x, visualGrid.m_extents.y, visualGrid.m_extents.z, verts, indices);
            // Weld verts in resulting mesh
            CustomMesh toWeld = new()
            {
                Triangles = indices.ToArray(),
                Vertices = verts.ToArray()
            };
            toWeld.GenerateHardNormals();
            toWeld.GenerateTriplanarUVs();
            MeshWelder welder = new(toWeld);
            //Profiling.Profiler.EndSample();

            // Welder returns tri indices in reverse order, for some reason
            welder.CustomMesh.Triangles = welder.CustomMesh.Triangles.Reverse().ToArray();
        
            Mesh mesh = new()
            {
                indexFormat = IndexFormat.UInt32
            };
            mesh.SetVertices(welder.CustomMesh.Vertices);
            mesh.SetTriangles(welder.CustomMesh.Triangles, 0);
            mesh.SetNormals(welder.CustomMesh.Normals);
            mesh.SetUVs(0, welder.CustomMesh.UVs);
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();

            // Multiple materials used like multi-pass rendering. Mesh for each material.
            foreach (Material material in UsedMaterials)
            {
                // TODO: Prefab for gem meshes?
                GameObject go = new($"Mesh_{material.name}")
                {
                    transform =
                    {
                        parent = transform
                    }
                };
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.GetComponent<Renderer>().material = material;
                go.GetComponent<MeshFilter>().mesh = mesh;
                go.layer = gameObject.layer;

                go.transform.localPosition = new Vector3(
                    -(float)visualGrid.m_extents.x / 2.0f, 
                    -(float)visualGrid.m_extents.y / 2.0f, 
                    -(float)visualGrid.m_extents.z / 2.0f);
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                m_meshes.Add(go);
            }
        }

        private Vector3 CoordToWorldPos(int x, int y, int z)
        {
            // We center the voxels on our transform, so we need to offset a given coord to get its world-space position
            // Start from zero, easier than figuring out if we need to add or subtract
            // .5,.5,.5 is b/c our cells are justified lower-left, we wanna treat center of cell as world position though
            Vector3 localPos = -(new Vector3(Size.x / 2.0f, Size.y / 2.0f, Size.z / 2.0f)) 
                   + (new Vector3(x, y, z)) + new Vector3(.5f,.5f,.5f);
            Transform xform = transform;
            return (Vector3)(xform.localToWorldMatrix * localPos) + xform.position;
        }

        public void Slice(Vector3 cutPlaneNormal)
        {
            Vector3 localCutNormal = Quaternion.Inverse(transform.rotation) * cutPlaneNormal;
            Vector3Int planePos = m_voxels.GetCoordinateFurthestAlongNormal(localCutNormal);
            m_voxels.SetValuesAtAndBeyondPlane(planePos, localCutNormal, m_voxels.m_defaultValue);
            UpdateMeshes();
            UpdateSlicePreview(cutPlaneNormal);
        }
        
        public void UpdateSlicePreview(Vector3 cutPlaneNormal)
        {
            Vector3 localCutNormal = Quaternion.Inverse(transform.rotation) * cutPlaneNormal;
            Vector3Int planePos = m_voxels.GetCoordinateFurthestAlongNormal(localCutNormal);
            Vector3 planeWorldPos = CoordToWorldPos(planePos.x, planePos.y, planePos.z);
            foreach (Material material in UsedMaterials)
            {
                material.SetVector(CutPlanePositionProperty, new Vector4(planeWorldPos.x, planeWorldPos.y, planeWorldPos.z, 0));
                material.SetVector(CutPlaneNormalProperty, new Vector4(cutPlaneNormal.x, cutPlaneNormal.y, cutPlaneNormal.z, 0));
            }
        }

        public bool Compare(GemStone other)
        {
            // Does this gem, in any orientation, match the provided gem?
            // 6 * 4 orientations to check (4 orientations of 6 faces)
            
            VoxelGrid thisCopy = m_voxels.Copy();
            thisCopy.Trim();
            VoxelGrid otherCopy = other.m_voxels.Copy();
            otherCopy.Trim();

            // Loop around the voxel grid, covering 4 faces
            for (int yIdx = 0; yIdx < 4; yIdx++)
            {
                // 4 orientations of one face of the voxel grid
                for (int xIdx = 0; xIdx < 4; xIdx++)
                {
                    if (thisCopy.Equals(otherCopy))
                    {
                        return true;
                    }
                    otherCopy.RotateX();
                }
                otherCopy.RotateY();
            }
            // Cover the last two faces
            otherCopy.RotateZ();
            for (int xIdx = 0; xIdx < 4; xIdx++)
            {
                if (thisCopy.Equals(otherCopy))
                {
                    return true;
                }
                otherCopy.RotateX();
            }
            otherCopy.RotateZ();
            otherCopy.RotateZ();
            for (int xIdx = 0; xIdx < 4; xIdx++)
            {
                if (thisCopy.Equals(otherCopy))
                {
                    return true;
                }
                otherCopy.RotateX();
            }

            return false;
        }

        #region Editor GUI
        public void OnGUI()
        {
            if (m_showVoxelValues && m_voxels != null)
            {
                DebugVoxelValues();
            }
        }
        
        private void OnDrawGizmos()
        {
            // Editor view that shows extents of gem, with optional slices into voxels
            Color gizmoColor = Selection.Contains(gameObject) ? Color.black : Color.yellow;
            // Dim gizmo during play, because there's probably a visualized mesh we wanna see!
            gizmoColor.a = Application.isPlaying ? .1f : .3f;
            Gizmos.color = gizmoColor;

            // Draw wireframe if selected
            if (Selection.Contains(gameObject))
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(Size.x, Size.y, Size.z));
            }
            else
            {
                Gizmos.DrawCube(transform.position, new Vector3(Size.x, Size.y, Size.z));
            }

            // Draw slice planes if enabled
            if (m_showGizmoPlanes)
            {
                float maxOpacity = .15f;
                // Fade an axis's slice planes as the camera approaches looking down the axis
                float axisDot = Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, new Vector3(1, 0, 0)));
                gizmoColor.a = maxOpacity * (1 - axisDot);
                Gizmos.color = gizmoColor;
                for (int x = 0; x < Size.x; x++)
                {
                    Vector3 pos = transform.position;
                    pos.x -= (-Size.x / 2.0f) + x;
                    Gizmos.DrawCube(pos, new Vector3(.01f, Size.y, Size.z));
                }
                axisDot = Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, new Vector3(0, 1, 0)));
                gizmoColor.a = maxOpacity * (1 - axisDot);
                Gizmos.color = gizmoColor;
                for (int y = 0; y < Size.y; y++)
                {
                    Vector3 pos = transform.position;
                    pos.y -= (-Size.y / 2.0f) + y;
                    Gizmos.DrawCube(pos, new Vector3(Size.x, .01f, Size.z));
                }
                axisDot = Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, new Vector3(0, 0, 1)));
                gizmoColor.a = maxOpacity * (1 - axisDot);
                Gizmos.color = gizmoColor;
                for (int z = 0; z < Size.z; z++)
                {
                    Vector3 pos = transform.position;
                    pos.z -= (-Size.z / 2.0f) + z;
                    Gizmos.DrawCube(pos, new Vector3(Size.x, Size.y, .01f));
                }
            }
        }

        private void DebugVoxelValues()
        {
            if (Camera.main is not {} cam)
            {
                return;
            }
                
            GUIStyle positiveStyle = new()
            {
                normal =
                {
                    textColor = Color.green
                }
            };
            GUIStyle negativeStyle = new()
            {
                normal =
                {
                    textColor = Color.red
                }
            };

            for (int idx = 0; idx < m_voxels.m_values.Length; idx++)
            {
                float val = m_voxels.m_values[idx];
                Vector3Int coords = m_voxels.IdxToCoord(idx);
                Vector3 pos = cam.WorldToScreenPoint(CoordToWorldPos(coords.x, coords.y, coords.z));
                GUI.Label(new Rect(pos.x, Screen.height-pos.y, 150, 130), 
                    val.ToString(CultureInfo.InvariantCulture),
                    val > 0 ? positiveStyle : negativeStyle);   
            }
        }
        #endregion
    }
}
