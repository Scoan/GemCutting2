using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MarchingCubesProject;
using System.Linq;
using GemStoneCatalog;
using System;
using External;
using UnityEngine.Rendering;

/// <summary>
/// Class representing a voxel grid which can be visualized as a gemstone.
/// </summary>
public class GemStone : MonoBehaviour {
    
    private static readonly int CutPlanePositionProperty = Shader.PropertyToID("_CutPlanePosition");
    private static readonly int CutPlaneNormalProperty = Shader.PropertyToID("_CutPlaneNormal");

    [Header("Debugging")]
    [SerializeField] private bool m_showGizmoPlanes = true;
    [Header("Generation Params")]
    [SerializeField] private List<Material> m_materials = new();
    [SerializeField] private GemTypes m_gemType = GemTypes.CUSTOM; //Override to create a gem from the catalog by default.
    [SerializeField] private Vector3Int m_size = new(7, 7, 7);
    [SerializeField] private int m_seed;
    [SerializeField] private float m_purityBias = .05f;  // 0 = ~50% voids, 50% solids. Higher = fewer voids.
    
    private Voxels.VoxelGrid m_voxels;
    private readonly List<GameObject> m_meshes = new();
    // TODO: Use. Instance materials!
    //private List<Material> m_materialInstances = new();
    public Voxels.VoxelGrid Voxels => m_voxels;

    // Necessary as members w/ getters or setters aren't exposed in editor :(
    public int Seed
    {
        set {
            if (m_seed != value)
            {
                m_seed = value;
                // When seed changes, regen voxels and mesh
                if (m_voxels != null)
                {
                    GenerateVoxelsAndMesh();
                }
            }
        }
        get => m_seed;
    }

    public Vector3Int Size
    {
        set
        {
            m_size = value;
            GenerateVoxelsAndMesh();
        }
        get => m_voxels?.m_extents ?? m_size;
    }

    public GemTypes GemType
    {
        get => m_gemType;
        set
        {
            m_gemType = value;
            GenerateVoxelsAndMesh();
        }
    }

    void Awake () {
        GenerateVoxelsAndMesh();
    }

    private void GenerateVoxelsAndMesh() {
        m_voxels = new Voxels.VoxelGrid(m_size);
        // TODO: Support a SEED type as well as a CUSTOM type
        // If seed, gen from seed. If custom, load from specified file? From a field?
        
        // Load catalog gem if one was specified!
        if (m_gemType != GemTypes.CUSTOM)
        {
            // Getting a type from a different assembly is fun. GemTypes needs to be in the same assembly as the types themselves.
            // Provide the fully qualified name to get the type!
            // TODO: Use assembly definition ya dink. This isn't necessary.
            Type gemClass = Type.GetType(typeof(GemTypes).Namespace + "." + m_gemType);
            if (gemClass != null)
            {
                m_voxels = (Voxels.VoxelGrid)Activator.CreateInstance(gemClass);
                UpdateMesh();
            }
        }
        // Otherwise, generate a random gem.
        else
        {
            m_voxels.GenerateFromSeed(m_seed, threshold: true, bias: m_purityBias);
            UpdateMesh();
        }
    }

    // TODO: This hitches. Thread creation of new mesh, only destroy & show new mesh once done?
    // Alternatively, cache tris for each voxel and only regenerate those which have changed?
    // TODO: Make private?
    public void UpdateMesh()
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
        
        // Pad the voxel grid to ensure we see edges.
        Voxels.VoxelGrid visualGrid = new Voxels.VoxelGrid(m_voxels.m_extents + new Vector3Int(3,3,3), m_voxels.m_defaultValue);
        visualGrid.CopyFrom(m_voxels, 2, 2, 2);

        //Profiling.Profiler.BeginSample("Gem - Marching Cubes");
        Marching marching = new MarchingCubes(0.0f);
        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();
        marching.Generate(visualGrid.m_values, visualGrid.m_extents.x, visualGrid.m_extents.y, visualGrid.m_extents.z, verts, indices);
        // Weld verts in resulting mesh
        CustomMesh toWeld = new CustomMesh
        {
            Triangles = indices.ToArray(),
            Vertices = verts.ToArray()
        };
        toWeld.GenerateHardNormals();
        toWeld.GenerateTriplanarUVs();
        MeshWelder welder = new MeshWelder(toWeld);
        //Profiling.Profiler.EndSample();

        // Welder returns tri indices in reverse order, for some reason
        welder.CustomMesh.Triangles = welder.CustomMesh.Triangles.Reverse().ToArray();
        
        Mesh mesh = new Mesh
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
        foreach (Material material in m_materials)
        {
            // TODO: Prefab for gem meshes?
            GameObject go = new GameObject("Mesh")
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

            go.transform.localPosition = new Vector3(-(float)visualGrid.m_extents.x / 2.0f, -(float)visualGrid.m_extents.y / 2.0f, -(float)visualGrid.m_extents.z / 2.0f);
            m_meshes.Add(go);
        }
    }

    public void UpdateSlicePreview(Vector3 planeNormal)
    {
        Vector3Int planePos = m_voxels.GetCoordinateFurthestAlongAngle(planeNormal);
        Vector3 planeWorldPos = CoordToWorldPos(planePos.x, planePos.y, planePos.z);
        foreach (Material material in m_materials)
        {
            material.SetVector(CutPlanePositionProperty, new Vector4(planeWorldPos.x, planeWorldPos.y, planeWorldPos.z, 0));
            material.SetVector(CutPlaneNormalProperty, new Vector4(planeNormal.x, planeNormal.y, planeNormal.z, 0));
        }
    }

    private void OnDrawGizmos()
    {
        // Editor view that shows extents of gem, with optional slices into voxels
        Color gizmoColor = Selection.Contains(this.gameObject) ? Color.black : Color.yellow;
        // Dim gizmo during play, because there's probably a visualized mesh we wanna see!
        gizmoColor.a = Application.isPlaying ? .1f : .3f;
        Gizmos.color = gizmoColor;

        // Draw wireframe if selected
        if (Selection.Contains(this.gameObject))
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

    public Vector3 CoordToWorldPos(int x, int y, int z)
    {
        // We center the voxels on our transform, so we need to offset a given coord to get its world-space position
        // Start from zero, easier than figuring out if we need to add or subtract
        // .5,.5,.5 is b/c our cells are justified lower-left, we wanna treat center of cell as world position though
        return transform.position - (new Vector3(Size.x / 2.0f, Size.y / 2.0f, Size.z / 2.0f)) 
               + (new Vector3(x, y, z)) + new Vector3(.5f,.5f,.5f);
    }
}
