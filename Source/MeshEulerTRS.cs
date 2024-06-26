using MeshTransformer.Utility;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MeshTransformer
{
    public class MeshEulerTRS : BaseMeshTransformer
    {
        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Vector3 rotation = Vector3.zero;
        [SerializeField] private Vector3 scale = Vector3.one;
        
        protected override void ResetTransform()
        {
            Debug.Log($"Mesh modified with next parameters: position - {position} ; rotation - {rotation} ; scale - {scale}");
            position = Vector3.zero;
            rotation = Vector3.zero;
            scale = Vector3.one;
        }
        protected override void UpdateVertex(ref Vector3 vector)
        {
            UpdateVertex1(ref vector);
        }

        private void UpdateVertex1(ref Vector3 vector)
        {
            vector = Vector3.Scale(vector, scale);
            var rotationMatrix = EulerAnglesExtensions.GetRotationMatrix1(rotation);
            vector = position + rotationMatrix * vector;
        }
        private void UpdateVertex2(ref Vector3 vector)
        {
            vector = Vector3.Scale(vector, scale);
            vector = EulerAnglesExtensions.GetRotateVector(vector, rotation);
            vector = position + vector;
        }
        private void UpdateVertex3(ref Vector3 vector)
        {
            vector = Vector3.Scale(vector, scale);
            var rotationMatrix = EulerAnglesExtensions.GetRotationMatrix2(rotation);
            vector = position + rotationMatrix * vector;
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(MeshEulerTRS))]
    public class MeshEulerTRSEditor : MeshTransformerEditor<MeshEulerTRS> { }
#endif
}