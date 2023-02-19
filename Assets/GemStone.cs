using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MarchingCubesProject;
using System.Linq;
using GemStoneCatalog;
using System;
using External;
using UnityEngine.Rendering;


public class GemStone : MonoBehaviour {
    /// <summary>
    /// Class representing a voxel grid which can be visualized as a gemstone.
    /// </summary>

    //TODO:
    // Handle resizes/any operations that change the grid's contents with an event? Need to redraw the mesh.

    public bool GizmoPlanes = true;

    public List<Material> m_materials = new();

    // When I originally wrote this, meshes had a ~32k vert limit, so we made a game object for each mesh
    // TODO: Update this. Do we still need multiple gameobjects/meshes?
    public List<GameObject> meshes = new List<GameObject>();

    //The size of the voxel aray on generation.
    public Vector3Int extents = new Vector3Int(7, 7, 7);
    public Voxels.VoxelGrid voxels;

    public GemTypes GemOverride = GemTypes.CUSTOM; //Override to create a gem from the catalog by default.

    // Values relating to random generation
    public int seed = 0;
    private int __seed = 0;     // Necessary as members w/ getters or setters aren't exposed in editor :(

    public float purityBias = .09f;  // 0 = ~50% voids, 50% solids. Higher = fewer voids.

    // Necessary as members w/ getters or setters aren't exposed in editor :(
    private int _seed
    {
        set {
            if (__seed != value)
            {
                __seed = value;
                // When seed changes, regen voxels and mesh
                if (voxels != null)
                {
                    voxels.GenerateFromSeed(__seed, threshold:true, bias:purityBias);
                    UpdateMesh();
                }
            }
        }
        get { return __seed; }
    }

    public int SizeX
    {
        set {
            if (voxels != null)
                voxels.extents.x = value;
            extents.x = value;
        }
        get { return voxels != null ? voxels.extents.x : extents.x; }
    }
    public int SizeY
    {
        set {
            if (voxels != null)
                voxels.extents.y = value;
            extents.y = value;
        }
        get { return voxels != null ? voxels.extents.y : extents.y; }
    }
    public int SizeZ
    {
        set
        {
            if (voxels != null)
                voxels.extents.z = value;
            extents.z = value;
        }
        get { return voxels != null ? voxels.extents.z : extents.z; }
    }

    // Use this for initialization
    void Awake () {
        InitializeFromProperties();
    }

	void OnValidate () {
        // Rebuild gem if any editor properties have changed
        // TODO: Figure out
        //InitializeFromEditorProperties();
        _seed = seed;
    }

    public void InitializeFromProperties() {
        voxels = new Voxels.VoxelGrid(extents, -1);
        // TODO: Support a SEED type as well as a CUSTOM type
        // If seed, gen from seed. If custom, load from specified file?
        // Load catalog gem if one was specified!
        if (GemOverride != GemTypes.CUSTOM)
        {
            // Getting a type from a different assembly is fun. GemTypes needs to be in the same assembly as the types themselves.
            // Provide the fully qualified name to get the type!
            Type gemClass = Type.GetType(typeof(GemTypes).Namespace + "." + GemOverride.ToString());
            voxels = (Voxels.VoxelGrid)Activator.CreateInstance(gemClass);
            UpdateMesh();
        }
        // Otherwise, generate a random gem.
        else
        {
            __seed = seed;
            voxels.GenerateFromSeed(__seed, threshold: true, bias: purityBias);
            UpdateMesh();
        }
    }

    // TODO: This hitches. Thread creation of new mesh, only destroy & show new mesh once done?
    // Alternatively, cache tris for each voxel and only regenerate those which have changed?
    public void UpdateMesh()
    {
        if (voxels == null)
        {
            return;
        }

        /// Generates a display mesh via marching cubes
        /// 
        Marching marching = new MarchingCubes(0.0f);

        // Nuke previous mesh
        foreach (GameObject visMesh in meshes)
        {
            // TODO: Get this to work in the editor
            //if (Application.isEditor)
            //{
            //    DestroyImmediate(visMesh);
            //}
            //else
            //{
            Destroy(visMesh);
            //} 
        }
        meshes.Clear();

        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();

        // Pad the voxel grid to ensure we see edges
        Voxels.VoxelGrid visualGrid = new Voxels.VoxelGrid(voxels.extents + new Vector3Int(3,3,3), voxels.defaultValue);
        visualGrid.CopyFrom(voxels, 2, 2, 2);
        //TODO: DON'T DO THIS. TEMPORARILY REMOVING PADDING UNTIL I FIGURE OUT WTF IS WRONG.
        //visualGrid = voxels;

        //The mesh produced is not optimal. There is one vert for each index.
        //Would need to weld vertices for better quality mesh.
        //Profiling.Profiler.BeginSample("My Sample");
        marching.Generate(visualGrid.values, visualGrid.extents.x, visualGrid.extents.y, visualGrid.extents.z, verts, indices);

        // Weld verts in resulting mesh
        CustomMesh toWeld = new CustomMesh();
        toWeld.Triangles = indices.ToArray();
        toWeld.Vertices = verts.ToArray();
        toWeld.GenerateHardNormals();
        toWeld.GenerateTriplanarUVs();
        MeshWelder welder = new MeshWelder(toWeld);
        //Profiling.Profiler.EndSample();

        // Welder returns tri indices in reverse order, for some reason
        welder.CustomMesh.Triangles = welder.CustomMesh.Triangles.Reverse().ToArray();
        
        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.SetVertices(welder.CustomMesh.Vertices);
        mesh.SetTriangles(welder.CustomMesh.Triangles, 0);
        mesh.SetNormals(welder.CustomMesh.Normals);
        mesh.SetUVs(0, welder.CustomMesh.UVs);
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        foreach (Material material in m_materials)
        {
            GameObject go = new GameObject("Mesh");
            go.transform.parent = transform;
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.GetComponent<Renderer>().material = material;
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.layer = this.gameObject.layer;

            go.transform.localPosition = new Vector3(-(float)visualGrid.extents.x / 2.0f, -(float)visualGrid.extents.y / 2.0f, -(float)visualGrid.extents.z / 2.0f);
            meshes.Add(go);
        }
    }

    public void UpdateSlicePreview(Vector3 planeNormal)
    {
        Vector3Int planePos = voxels.GetCoordinateFurthestAlongAngle(planeNormal);
        Vector3 planeWorldPos = CoordToWorldPos(planePos.x, planePos.y, planePos.z);
        foreach (Material material in m_materials)
        {
            material.SetVector("_CutPlanePosition", new Vector4(planeWorldPos.x, planeWorldPos.y, planeWorldPos.z, 0));
            material.SetVector("_CutPlaneNormal", new Vector4(planeNormal.x, planeNormal.y, planeNormal.z, 0));
        }
    }

    private void OnDrawGizmos()
    {
        /// Editor view that shows extents of gem, with optional slices into voxels
        Color gizmoColor = Selection.Contains(this.gameObject) ? Color.black : Color.yellow;
        // Dim gizmo during play, because there's probably a visualized mesh we wanna see!
        gizmoColor.a = Application.isPlaying ? .1f : .3f;
        Gizmos.color = gizmoColor;

        // Draw wireframe if selected
        if (Selection.Contains(this.gameObject))
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(SizeX, SizeY, SizeZ));
        }
        else
        {
            Gizmos.DrawCube(transform.position, new Vector3(SizeX, SizeY, SizeZ));
        }

        // Draw slice planes if enabled
        if (GizmoPlanes)
        {
            float maxOpacity = .15f;
            // Fade an axis's slice planes as the camera approaches looking down the axis
            float axisDot = Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, new Vector3(1, 0, 0)));
            gizmoColor.a = maxOpacity * (1 - axisDot);
            Gizmos.color = gizmoColor;
            for (int x = 0; x < SizeX; x++)
            {
                Vector3 pos = transform.position;
                pos.x -= (-SizeX / 2.0f) + x;
                Gizmos.DrawCube(pos, new Vector3(.01f, SizeY, SizeZ));
            }
            axisDot = Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, new Vector3(0, 1, 0)));
            gizmoColor.a = maxOpacity * (1 - axisDot);
            Gizmos.color = gizmoColor;
            for (int y = 0; y < SizeY; y++)
            {
                Vector3 pos = transform.position;
                pos.y -= (-SizeY / 2.0f) + y;
                Gizmos.DrawCube(pos, new Vector3(SizeX, .01f, SizeZ));
            }
            axisDot = Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, new Vector3(0, 0, 1)));
            gizmoColor.a = maxOpacity * (1 - axisDot);
            Gizmos.color = gizmoColor;
            for (int z = 0; z < SizeZ; z++)
            {
                Vector3 pos = transform.position;
                pos.z -= (-SizeZ / 2.0f) + z;
                Gizmos.DrawCube(pos, new Vector3(SizeX, SizeY, .01f));
            }
        }
    }

    public Vector3 CoordToWorldPos(int x, int y, int z)
    {
        // We center the voxels on our transform, so we need to offset a given coord to get its world-space position
        // Start from zero, easier than figuring out if we need to add or subtract
        // .5,.5,.5 is b/c our cells are justifed lower-left, we wanna treat center of cell as world position though
        return this.transform.position - (new Vector3(SizeX / 2.0f, SizeY / 2.0f, SizeZ / 2.0f)) + (new Vector3(x, y, z)) + new Vector3(.5f,.5f,.5f);
    }
}
