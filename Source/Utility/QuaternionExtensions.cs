using UnityEngine;

namespace MeshTransformer.Utility
{
    public static class QuaternionExtensions
    {
        public static Quaternion NormalizeQuaternion(Vector3 axis, float angle)
        {
            var normalizedAxis = axis.normalized;
            var halfAngle = angle * 0.5f * Mathf.Deg2Rad;
            
            return new Quaternion
            {
                w = Mathf.Cos(halfAngle),
                x = normalizedAxis.x * Mathf.Sin(halfAngle),
                y = normalizedAxis.y * Mathf.Sin(halfAngle),
                z = normalizedAxis.z * Mathf.Sin(halfAngle),
            };
        }
        public static Vector3 RotateVector(Quaternion q, Vector3 v)
        {
            var p = Inverse(q);
            var t = Multiply(q, v);
            t = Multiply(t, p);
            return new Vector3(t.x, t.y, t.z);
        }
        
        public static Quaternion Multiply(Quaternion q1, Quaternion q2)
        {
            return new Quaternion
            {
                w = (q1.w * q2.w) - (q1.x * q2.x) - (q1.y * q2.y) - (q1.z * q2.z),
                x = (q1.w * q2.x) + (q1.x * q2.w) + (q1.y * q2.z) - (q1.z * q2.y),
                y = (q1.w * q2.y) - (q1.x * q2.z) + (q1.y * q2.w) + (q1.z * q2.x),
                z = (q1.w * q2.z) + (q1.x * q2.y) - (q1.y * q2.x) + (q1.z * q2.w),
            };
        }
        public static Quaternion Multiply(Quaternion q, Vector3 v)
        {
            return new Quaternion
            {
                w = -(q.x * v.x) - (q.y * v.y) - (q.z * v.z),
                x =  (q.w * v.x) + (q.y * v.z) - (q.z * v.y),
                y =  (q.w * v.y) - (q.x * v.z) + (q.z * v.x),
                z =  (q.w * v.z) + (q.x * v.y) - (q.y * v.x),
            };
        }
        
        public static Quaternion Inverse(Quaternion q)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            return q;
        }
        
        
        public static Vector3 RotateVectorOptimized(Quaternion rotation, Vector3 point)
        {
            var num1 = rotation.x * 2f;
            var num2 = rotation.y * 2f;
            var num3 = rotation.z * 2f;
            
            var num4 = rotation.x * num1;
            var num5 = rotation.y * num2;
            var num6 = rotation.z * num3;
            
            var num7 = rotation.x * num2;
            var num8 = rotation.x * num3;
            var num9 = rotation.y * num3;
            
            var num10 = rotation.w * num1;
            var num11 = rotation.w * num2;
            var num12 = rotation.w * num3;
            
            Vector3 vector3;
            vector3.x = (1.0f - (num5 + num6)) * point.x
                        + (num7 - num12) * point.y 
                        + (num8 + num11) * point.z;
            vector3.y = (num7 + num12) * point.x 
                        + (1.0f - (num4 + num6)) * point.y
                        + (num9 - num10) * point.z;
            vector3.z = (num8 - num11) * point.x 
                        + (num9 + num10) * point.y 
                        + (1.0f - (num4 + num5)) * point.z;
            
            return vector3;
        }
    }
}