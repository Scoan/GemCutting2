using System;
using System.Collections.Generic;
using UnityEngine;

/*
	Originally created by Bunny83.
	Was streamlined and modified a bit by Terrev to better fit the needs of this project,
	then overhauled by grappigegovert for major speed improvements.
	Bunny83's original version:
	https://www.dropbox.com/s/u0wfq42441pkoat/MeshWelder.cs?dl=0
	Which was posted here:
	http://answers.unity3d.com/questions/1382854/welding-vertices-at-runtime.html
*/

namespace External
{
    [Flags]
    public enum EVertexAttribute
    {
        Position = 0x0001,
        Normal = 0x0002,
        UV1 = 0x0010,
    }

    public class Vertex
    {
        public readonly Vector3 Pos;
        public readonly Vector3 Normal;
        public Vector2 UV1;

        public Vertex(Vector3 pos, Vector3 normal)
        {
            Pos = pos;
            Normal = normal;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vertex other)
            {
                return other.Pos.Equals(Pos) && other.Normal.Equals(Normal) && other.UV1.Equals(UV1);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Pos.x.GetHashCode();
                hashCode = (hashCode * 397) ^ Pos.y.GetHashCode();
                hashCode = (hashCode * 397) ^ Pos.z.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.x.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.z.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.y.GetHashCode();
                return hashCode;
            }
        }
    }
    
    public class CustomMesh
    {
        public int[] Triangles;
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector2[] UVs;
        public int Material;

        // Generate hard normals from the provided verts
        public void GenerateHardNormals()
        {
            List<Vector3> normals = new();
            for (int idx = 0; idx < Vertices.Length; idx += 3)
            {
                Vector3 vertA = Vertices[idx];
                Vector3 vertB = Vertices[idx+1];
                Vector3 vertC = Vertices[idx+2];
                Vector3 normal = Vector3.Cross(vertB - vertC, vertB - vertA).normalized;
                normals.AddRange(new List<Vector3>() {normal, normal, normal});
            }

            Normals = normals.ToArray();
        }

        // Generate triplanar UVs from from provided verts and normals
        public void GenerateTriplanarUVs()
        {
            List<Vector2> uvs = new();
            if (Vertices.Length != Normals.Length)
            {
                return;
            }
            for (int idx = 0; idx < Vertices.Length; idx++)
            {
                Vector2 uv = new();
                Vector3 absNormal = new Vector3(Mathf.Abs(Normals[idx].x), Mathf.Abs(Normals[idx].y), Mathf.Abs(Normals[idx].z));
                // Get major axis for the current vert's normal
                int majorAxis = absNormal.x >= absNormal.y ? 
                    absNormal.x >= absNormal.z ? 0 : 2 
                    : absNormal.y >= absNormal.z ? 1 : 2;
                switch (majorAxis)
                {
                    case 0: // YZ UVs
                        uv.x = Vertices[idx].z * Mathf.Sign(Normals[idx].x);
                        uv.y = Vertices[idx].y;
                        break;
                    case 1: // XZ UVs
                        uv.x = Vertices[idx].x * Mathf.Sign(Normals[idx].y);
                        uv.y = Vertices[idx].z;
                        break;
                    case 2: // XY UVs
                        uv.x = Vertices[idx].x * -Mathf.Sign(Normals[idx].z);
                        uv.y = Vertices[idx].y;
                        break;
                }
                uvs.Add(uv);
            }

            UVs = uvs.ToArray();
        }
    }

    public class MeshWelder
    {
        private Vertex[] m_vertices;
        private Dictionary<Vertex, List<int>> m_newVerts;
        private int[] m_map;

        private EVertexAttribute m_attributes;
        public readonly CustomMesh CustomMesh;

        public MeshWelder(CustomMesh inputMesh)
        {
            CustomMesh = inputMesh;
            Weld();
        }

        private bool HasAttr(EVertexAttribute aAttr)
        {
            return (m_attributes & aAttr) != 0;
        }

        private void CreateVertexList()
        {
            Vector3[] positions = CustomMesh.Vertices;
            Vector3[] normals = CustomMesh.Normals;
            Vector2[] uv1 = CustomMesh.UVs;
            m_attributes = EVertexAttribute.Position;
            if (normals != null && normals.Length > 0) m_attributes |= EVertexAttribute.Normal;
            if (uv1 != null && uv1.Length > 0) m_attributes |= EVertexAttribute.UV1;

            m_vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                var v = new Vertex(positions[i], normals[i]);
                if (HasAttr(EVertexAttribute.UV1)) v.UV1 = uv1[i];
                m_vertices[i] = v;
            }
        }

        private void RemoveDuplicates()
        {
            m_newVerts = new Dictionary<Vertex, List<int>>(m_vertices.Length);
            for(int i = 0; i < m_vertices.Length; i++)
            {
                Vertex v = m_vertices[i];
                List<int> originals;
                if (m_newVerts.TryGetValue(v, out originals))
                {
                    originals.Add(i);
                }
                else
                {
                    m_newVerts.Add(v, new List<int> {i});
                }
            }
        }

        private void AssignNewVertexArrays()
        {
            m_map = new int[m_vertices.Length];
            CustomMesh.Vertices = new Vector3[m_newVerts.Count];
            CustomMesh.Normals = new Vector3[m_newVerts.Count];
            if (HasAttr(EVertexAttribute.UV1))
                CustomMesh.UVs = new Vector2[m_newVerts.Count];
            int i = 0;
            foreach (KeyValuePair<Vertex, List<int>> kvp in m_newVerts)
            {
                foreach (int index in kvp.Value)
                {
                    m_map[index] = i;
                }
                CustomMesh.Vertices[i] = kvp.Key.Pos;
                CustomMesh.Normals[i] = kvp.Key.Normal;
                if (HasAttr(EVertexAttribute.UV1))
                    CustomMesh.UVs[i] = kvp.Key.UV1;
                i++;
            }
        }

        private void RemapTriangles()
        {
            int[] tris = CustomMesh.Triangles;
            for (int i = 0; i < tris.Length; i++)
            {
                tris[i] = m_map[tris[i]];
            }
            CustomMesh.Triangles = tris;
        }

        public void Weld()
        {
            CreateVertexList();
            RemoveDuplicates();
            AssignNewVertexArrays();
            RemapTriangles();
        }
    }
}