using UnityEngine;

namespace MeshTransformer.Utility
{
    public static class VectorExtensions
    {
        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x +
                   a.y * b.y +
                   a.z * b.z;
        }
        
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.y * b.z - a.z * b.y, 
                a.z * b.x - a.x * b.z, 
                a.x * b.y - a.y * b.x);
        }
    }
}