using System;
using MeshTransformer.Utility;
using UnityEditor;
using UnityEngine;

namespace MeshTransformer
{
    public class MeshQuaternion : BaseMeshTransformer
    {
        [SerializeField] private Vector3 axis = Vector3.zero;
        [SerializeField] private float angle;

        protected override void ResetTransform()
        {
            Debug.Log($"Mesh modified with next parameters: axis - {axis} ; angle - {angle}");
            axis = Vector3.zero;
            angle = 0f;
        }

        protected override void UpdateVertex(ref Vector3 vector)
        {
            var normalizedAxis = axis.normalized;
            var halfAngle = angle * 0.5f * Mathf.Deg2Rad;
            
            var q = new Quaternion
            {
                x = normalizedAxis.x * Mathf.Sin(halfAngle),
                y = normalizedAxis.y * Mathf.Sin(halfAngle),
                z = normalizedAxis.z * Mathf.Sin(halfAngle),
                w = Mathf.Cos(halfAngle)
            };
            
            vector = QuaternionExtensions.RotateVector(q, vector);
        }
        
        
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(MeshQuaternion))]
    public class MeshQuaternionEditor : MeshTransformerEditor<MeshQuaternion> { }
#endif
}