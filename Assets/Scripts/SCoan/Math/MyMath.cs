using UnityEngine;

namespace SCoan.Math
{
    public static class MyMath
    {
        // TODO: Provide DistanceSquaredToPlane()?
        public static float DistanceToPlane(Vector3 pt, Vector3 planePos, Vector3 planeNormal)
        {
            planeNormal = planeNormal.normalized;
            // Given a point and a plane defined by (pos, normal), returns distance from the pt to the plane
            // per https://mathinsight.org/distance_point_plane
            float dee = -(planeNormal.x * planePos.x + planeNormal.y * planePos.y + planeNormal.z * planePos.z);

            float distToPlane = planeNormal.x * pt.x + planeNormal.y * pt.y + planeNormal.z * pt.z + dee;
            distToPlane /= Mathf.Sqrt(planeNormal.x * planeNormal.x + planeNormal.y * planeNormal.y + planeNormal.z * planeNormal.z);

            return distToPlane;
        }
    }
}


