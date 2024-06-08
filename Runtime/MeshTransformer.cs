using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MeshTransformer
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshTransformer : MonoBehaviour
    {
        [SerializeField] private Vector3 position = new Vector3(0, 0, 0);
        [SerializeField] private Vector3 rotation = new Vector3(0, 0, 0);
        [SerializeField] private Vector3 scale = new Vector3(1, 1, 1);
        [Space] [SerializeField] private Mesh origin;

        private void SaveMesh()
        {
            origin.SetVertices(GetTransformedVertices());
            Debug.Log($"Mesh modified with next parameters: position - {position} ; rotation - {rotation} ; scale - {scale}");
            position = new Vector3(0, 0, 0);
            rotation = new Vector3(0, 0, 0);
            scale = new Vector3(1, 1, 1);
        }

        private Vector3[] GetTransformedVertices()
        {
            var originVertices = origin.vertices;
            for (var i = 0; i < originVertices.Length; i++)
                UpdateVertex3(ref originVertices[i]);
            return originVertices;
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
        
        #if UNITY_EDITOR
        [CustomEditor(typeof(MeshTransformer))]
        private class MeshTransformerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                var data = (MeshTransformer)target;

                if (!data.origin.isReadable) return;
                var mesh = new Mesh
                {
                    vertices = data.GetTransformedVertices(),
                    triangles = data.origin.triangles
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
}