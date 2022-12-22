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
            var vertices = new Vector3[originVertices.Length];
            var eulerAngles = rotation * Mathf.Deg2Rad;
            for (var i = 0; i < originVertices.Length; i++)
            {
                var scalePos = Vector3.Scale(originVertices[i], scale);
                
                // X axis
                var y1 = Mathf.Cos(eulerAngles.x) * scalePos.y - Mathf.Sin(eulerAngles.x) * scalePos.z;
                var z1 = Mathf.Sin(eulerAngles.x) * scalePos.y + Mathf.Cos(eulerAngles.x) * scalePos.z;
                // Y axis
                var x1 = Mathf.Cos(eulerAngles.z) * scalePos.x - Mathf.Sin(eulerAngles.z) * y1;
                var y2 = Mathf.Sin(eulerAngles.z) * scalePos.x + Mathf.Cos(eulerAngles.z) * y1;
                // Z axis
                var x2 = Mathf.Cos(eulerAngles.y) * x1 + Mathf.Sin(eulerAngles.y) * z1;
                var z2 = Mathf.Cos(eulerAngles.y) * z1 - Mathf.Sin(eulerAngles.y) * x1;
                
                vertices[i] = position + new Vector3(x2, y2, z2);
            }
            return vertices;
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