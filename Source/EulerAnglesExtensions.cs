using UnityEngine;

namespace MeshTransformer
{
    public static class EulerAnglesExtensions
    {
        public static Matrix3x3 GetRotationMatrix1(Vector3 angles)
        {
            angles *= Mathf.Deg2Rad;
            angles = -angles;
            
            float sinX = Mathf.Sin(angles.x), cosX = Mathf.Cos(angles.x);
            float sinY = Mathf.Sin(angles.y), cosY = Mathf.Cos(angles.y);
            float sinZ = Mathf.Sin(angles.z), cosZ = Mathf.Cos(angles.z);
            
            var x = new Matrix3x3
            {
                m00 = 1, m10 =    0, m20 =     0,
                m01 = 0, m11 = cosX, m21 = -sinX,
                m02 = 0, m12 = sinX, m22 =  cosX,
            };
            var y = new Matrix3x3
            {
                m00 =  cosY, m10 = 0, m20 = sinY,
                m01 =     0, m11 = 1, m21 =    0,
                m02 = -sinY, m12 = 0, m22 = cosY,
            };
            var z = new Matrix3x3
            {
                m00 = cosZ, m10 = -sinZ, m20 = 0,
                m01 = sinZ, m11 =  cosZ, m21 = 0,
                m02 =    0, m12 =     0, m22 = 1,
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
            
            float sinX = Mathf.Sin(angles.x), cosX = Mathf.Cos(angles.x);
            float sinY = Mathf.Sin(angles.y), cosY = Mathf.Cos(angles.y);
            float sinZ = Mathf.Sin(angles.z), cosZ = Mathf.Cos(angles.z);
            
            // X axis
            var y1 = cosX * vector3.y - sinX * vector3.z;
            var z1 = sinX * vector3.y + cosX * vector3.z;
            // Y axis
            var x1 = cosZ * vector3.x - sinZ * y1;
            var y2 = sinZ * vector3.x + cosZ * y1;
            // Z axis
            var x2 = cosY * x1 + sinY * z1;
            var z2 = cosY * z1 - sinY * x1;

            return new Vector3(x2, y2, z2);
        }
    }
}