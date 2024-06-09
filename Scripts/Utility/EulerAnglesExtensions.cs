using UnityEngine;

namespace MeshTransformer.Utility
{
    public static class EulerAnglesExtensions
    {
        public static Matrix3x3 GetRotationMatrix1(Vector3 angles)
        {
            angles *= Mathf.Deg2Rad;
            angles = -angles;
            
            var x = new Matrix3x3
            {
                m00 = 1, m10 =                   0, m20 =                    0,
                m01 = 0, m11 = Mathf.Cos(angles.x), m21 = -Mathf.Sin(angles.x),
                m02 = 0, m12 = Mathf.Sin(angles.x), m22 =  Mathf.Cos(angles.x),
            };
            var y = new Matrix3x3
            {
                m00 =  Mathf.Cos(angles.y), m10 = 0, m20 = Mathf.Sin(angles.y),
                m01 =                    0, m11 = 1, m21 =                   0,
                m02 = -Mathf.Sin(angles.y), m12 = 0, m22 = Mathf.Cos(angles.y),
            };
            var z = new Matrix3x3
            {
                m00 = Mathf.Cos(angles.z), m10 = -Mathf.Sin(angles.z), m20 = 0,
                m01 = Mathf.Sin(angles.z), m11 =  Mathf.Cos(angles.z), m21 = 0,
                m02 =                   0, m12 =                    0, m22 = 1,
            };
            
            return x * y * z;
        }
        public static Matrix3x3 GetRotationMatrix2(Vector3 angles)
        {
            angles *= Mathf.Deg2Rad;
            angles = -angles;

            float sinX = Mathf.Sin(angles.x), cosX = Mathf.Cos(angles.x);
            float sinY = Mathf.Sin(angles.y), cosY = Mathf.Cos(angles.y);
            float sinZ = Mathf.Sin(angles.z), cosZ = Mathf.Cos(angles.z);
            
            var result = new Matrix3x3
            {
                m00 = cosY * cosZ, m10 = sinX * sinY * cosZ - cosX * sinZ, m20 = cosX * sinY * cosZ + sinX * sinZ,
                m01 = cosY * sinZ, m11 = sinX * sinY * sinZ + cosX * cosZ, m21 = cosX * sinY * sinZ - sinX * cosZ,
                m02 =       -sinY, m12 =                      sinX * cosY, m22 =                      cosX * cosY,
            };
            
            return result;
        }
        public static Vector3 GetRotateVector(Vector3 vector3, Vector3 angles)
        {
            angles *= Mathf.Deg2Rad;
            // X axis
            var y1 = Mathf.Cos(angles.x) * vector3.y - Mathf.Sin(angles.x) * vector3.z;
            var z1 = Mathf.Sin(angles.x) * vector3.y + Mathf.Cos(angles.x) * vector3.z;
            // Y axis
            var x1 = Mathf.Cos(angles.z) * vector3.x - Mathf.Sin(angles.z) * y1;
            var y2 = Mathf.Sin(angles.z) * vector3.x + Mathf.Cos(angles.z) * y1;
            // Z axis
            var x2 = Mathf.Cos(angles.y) * x1 + Mathf.Sin(angles.y) * z1;
            var z2 = Mathf.Cos(angles.y) * z1 - Mathf.Sin(angles.y) * x1;

            return new Vector3(x2, y2, z2);
        }
    }
}