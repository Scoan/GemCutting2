using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using PlayerInputUtils;
using ProceduralNoiseProject;
using B83.MeshHelper;

namespace MarchingCubesProject
{

    public enum MARCHING_MODE {  CUBES, TETRAHEDRON };

    public class Example : MonoBehaviour
    {

        public Material m_material;

        public MARCHING_MODE mode = MARCHING_MODE.CUBES;

        public int seed = 0;

        List<GameObject> meshes = new List<GameObject>();

        int GetIdx(int x, int y, int z, int width, int height, int length)
        {
            return x + y * width + z * width * height;
        }


        void Start()
        {
            //MyPlayerInput.Start();

            INoise perlin = new PerlinNoise(seed, 10.0f);
            FractalNoise fractal = new FractalNoise(perlin, 1, 1.0f);

            //Set the mode used to create the mesh.
            //Cubes is faster and creates less verts, tetrahedrons is slower and creates more verts but better represents the mesh surface.
            Marching marching = null;
            if(mode == MARCHING_MODE.TETRAHEDRON)
                marching = new MarchingTertrahedron(0.0f);
            else
                marching = new MarchingCubes(0.0f);

            //Surface is the value that represents the surface of mesh
            //For example the perlin noise has a range of -1 to 1 so the mid point is where we want the surface to cut through.
            //The target value does not have to be the mid point it can be any value with in the range.
            marching.Surface = 0.0f;

            //The size of voxel array.
            int width = 9;
            int height = 9;
            int length = 9;

            float[] voxels = new float[width * height * length];

            //Fill voxels with values. Im using perlin noise but any method to create voxels will work.
            //for (int x = 0; x < width; x++)
            //{
            //    for (int y = 0; y < height; y++)
            //    {
            //        for (int z = 0; z < length; z++)
            //        {
            //            float fx = x / (width - 1.0f);
            //            float fy = y / (height - 1.0f);
            //            float fz = z / (length - 1.0f);

            //            int idx = x + y * width + z * width * height;

            //            voxels[idx] = fractal.Sample3D(fx, fy, fz);
            //        }
            //    }
            //}
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        int idx = GetIdx(x, y, z, width, height, length);

                        // Guarantee the edges are empty
                        if (x == 0 || y == 0 || z == 0)
                        {
                            voxels[idx] = -1;
                            continue;
                        }
                        else if (x == width-1 || y == height-1 || z == length-1)
                        {
                            voxels[idx] = -1;
                            continue;
                        }
                        else
                        {
                            voxels[idx] = 1;
                        }

                        // Threshold'd perlin noise

                        float fx = x / (width - 1.0f);
                        float fy = y / (height - 1.0f);
                        float fz = z / (length - 1.0f);

                        float val = fractal.Sample3D(fx, fy, fz);

                        
                        voxels[idx] = val > -.2 ? 1 : -1;
                        
                        // Some 0-vals
                        if (Mathf.Abs(val) < .01)
                        {
                            //Debug.Log(val);
                            voxels[idx] = 0;
                        }

                        // TODO:


                        //voxels[idx] = val;

                        //float rand = Random.value;
                        //voxels[idx] = rand > .5f ? 1 : -1;
                    }
                }
            }

            //stupid
            //voxels[4] = 0;
            //voxels[10] = 0;
            //voxels[12] = 0;
            //voxels[13] = 1;
            //voxels[14] = 0;
            //voxels[16] = 0;
            //voxels[22] = 0;


            List<Vector3> verts = new List<Vector3>();
            List<int> indices = new List<int>();

            //The mesh produced is not optimal. There is one vert for each index.
            //Would need to weld vertices for better quality mesh.
            marching.Generate(voxels, width, height, length, verts, indices);

            //A mesh in unity can only be made up of 65000 verts.
            //Need to split the verts between multiple meshes.

            int maxVertsPerMesh = 30000; //must be divisible by 3, ie 3 verts == 1 triangle
            int numMeshes = verts.Count / maxVertsPerMesh + 1;

            for (int i = 0; i < numMeshes; i++)
            {

                List<Vector3> splitVerts = new List<Vector3>();
                List<int> splitIndices = new List<int>();

                for (int j = 0; j < maxVertsPerMesh; j++)
                {
                    int idx = i * maxVertsPerMesh + j;

                    if (idx < verts.Count)
                    {
                        splitVerts.Add(verts[idx]);
                        splitIndices.Add(j);
                    }
                }

                if (splitVerts.Count == 0) continue;

                Mesh mesh = new Mesh();
                mesh.SetVertices(splitVerts);
                mesh.SetTriangles(splitIndices, 0);
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();

                //MeshWelder welder = new MeshWelder(mesh);
                //welder.MaxPositionDelta = .1f;
                //welder.Weld();

                GameObject go = new GameObject("Mesh");
                go.transform.parent = transform;
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                go.GetComponent<Renderer>().material = m_material;
                go.GetComponent<MeshFilter>().mesh = mesh;

                go.transform.localPosition = new Vector3(-width / 2, -height / 2, -length / 2);
                meshes.Add(go);
            }

        }

        void Update()
        {
            //MyPlayerInput.Update();
            //transform.Rotate(Vector3.up, 5.0f * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            //MyPlayerInput.FixedUpdate();
        }

    }

}
