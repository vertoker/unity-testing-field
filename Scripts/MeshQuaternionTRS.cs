using MeshTransformer.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace MeshTransformer
{
    public class MeshQuaternionTRS : BaseMeshTransformer
    {
        [SerializeField] private Vector3 position = new Vector3(0, 0, 0);
        [SerializeField] private Vector3 rotationAxis = Vector3.zero;
        [SerializeField] private float rotationAngle;
        [SerializeField] private Vector3 scale = Vector3.one;

        protected override void ResetTransform()
        {
            Debug.Log($"Mesh modified with next parameters: " +
                      $"position - {position} ; scale - {scale} ; " +
                      $"rotationAxis - {rotationAxis} ; rotationAngle - {rotationAngle}");
            
            position = Vector3.zero;
            rotationAxis = Vector3.zero;
            rotationAngle = 0f;
            rotationAxis = Vector3.one;
        }

        protected override void UpdateVertex(ref Vector3 vector)
        {
            var q = QuaternionExtensions.NormalizeQuaternion(rotationAxis, rotationAngle);
            
            vector = Vector3.Scale(vector, scale);
            vector = QuaternionExtensions.RotateVector(q, vector);
            vector = position + vector;
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(MeshQuaternionTRS))]
    public class MeshQuaternionTRSEditor : MeshTransformerEditor<MeshQuaternionTRS> { }
#endif
}