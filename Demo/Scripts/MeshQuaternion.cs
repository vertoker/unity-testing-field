using UnityEditor;
using UnityEngine;

namespace MeshTransformer.Demo
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
            var q = QuaternionExtensions.NormalizeQuaternion(axis, angle);
            vector = QuaternionExtensions.RotateVector(q, vector);
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(MeshQuaternion))]
    public class MeshQuaternionEditor : MeshTransformerEditor<MeshQuaternion> { }
#endif
}