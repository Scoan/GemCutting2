using UnityEngine;

namespace SCoan.Mesh
{
    public static class MeshExtensions
    {
        
        // Generate triplanar UVs
        public static void GenerateTriplanarUVs(this UnityEngine.Mesh mesh, int uvIndex)
        {
            Vector2[] triplanarUVs = new Vector2[mesh.vertexCount];
            
            Vector3[] meshVerts = mesh.vertices;
            Vector3[] meshNormals = mesh.normals;
            
            for (int idx = 0; idx < meshVerts.Length; idx++)
            {
                Vector2 uv = new();
                Vector3 absNormal = new(
                    Mathf.Abs(meshNormals[idx].x), 
                    Mathf.Abs(meshNormals[idx].y), 
                    Mathf.Abs(meshNormals[idx].z));
                // Get major axis for the current vert's normal
                int majorAxis = absNormal.x >= absNormal.y ? 
                    absNormal.x >= absNormal.z ? 0 : 2 
                    : absNormal.y >= absNormal.z ? 1 : 2;
                switch (majorAxis)
                {
                    case 0: // YZ UVs
                        uv.x = meshVerts[idx].z * Mathf.Sign(meshNormals[idx].x);
                        uv.y = meshVerts[idx].y;
                        break;
                    case 1: // XZ UVs
                        uv.x = meshVerts[idx].x * Mathf.Sign(meshNormals[idx].y);
                        uv.y = meshVerts[idx].z;
                        break;
                    case 2: // XY UVs
                        uv.x = meshVerts[idx].x * -Mathf.Sign(meshNormals[idx].z);
                        uv.y = meshVerts[idx].y;
                        break;
                }
                triplanarUVs[idx] = uv;
            }
            
            mesh.SetUVs(uvIndex, triplanarUVs);
        }
        
        // Generate hard normals
        public static void GenerateHardNormals(this UnityEngine.Mesh mesh)
        {
            Vector3[] hardNormals = new Vector3[mesh.vertexCount];
            
            Vector3[] meshVerts = mesh.vertices;
            
            for (int idx = 0; idx < meshVerts.Length; idx += 3)
            {
                Vector3 vertA = meshVerts[idx];
                Vector3 vertB = meshVerts[idx+1];
                Vector3 vertC = meshVerts[idx+2];
                Vector3 normal = Vector3.Cross(vertB - vertC, vertB - vertA).normalized;

                hardNormals[idx] = normal;
                hardNormals[idx+1] = normal;
                hardNormals[idx+2] = normal;
            }

            mesh.SetNormals(hardNormals);
        }
    }
}