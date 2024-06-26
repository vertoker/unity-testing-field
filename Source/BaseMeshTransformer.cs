using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MeshTransformer
{
    [RequireComponent(typeof(MeshFilter))]
    public abstract class BaseMeshTransformer : MonoBehaviour
    {
        [SerializeField] private Mesh origin;

        public Mesh Origin => origin;

        public void SaveMesh()
        {
            origin.SetVertices(GetTransformedVertices());
#if UNITY_EDITOR
            EditorUtility.SetDirty(origin);
#endif
            ResetTransform();
        }
        public Vector3[] GetTransformedVertices()
        {
            var originVertices = origin.vertices;
            for (var i = 0; i < originVertices.Length; i++)
                UpdateVertex(ref originVertices[i]);
            return originVertices;
        }

        protected abstract void ResetTransform();
        protected abstract void UpdateVertex(ref Vector3 vector);
    }
    
#if UNITY_EDITOR
    public abstract class MeshTransformerEditor<TTransformer> : Editor
        where TTransformer : BaseMeshTransformer
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var data = (TTransformer)target;

            if (!data.Origin.isReadable) return;
            var mesh = new Mesh
            {
                vertices = data.GetTransformedVertices(),
                triangles = data.Origin.triangles
            };
            data.GetComponent<MeshFilter>().mesh = mesh;
                
            if (GUILayout.Button("Modify Mesh (MAKE DUPLICATES)"))
            {
                data.SaveMesh();
            }
        }
    }
#endif
}