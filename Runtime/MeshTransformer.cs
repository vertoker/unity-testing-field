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
                UpdateVertex(ref originVertices[i]);
            return originVertices;
        }

        private void UpdateVertex(ref Vector3 vector)
        {
            vector = Vector3.Scale(vector, scale);
            var rotationMatrix = GetRotationMatrix(rotation);
            vector = position + rotationMatrix * vector;
        }

        private static Matrix3x3 GetRotationMatrix(Vector3 angles)
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

        private static Vector3 GetRotateVector(Vector3 vector3, Vector3 angles)
        {
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