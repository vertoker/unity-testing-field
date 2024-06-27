using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Splines
{
    public class SplineJoints : MonoBehaviour
    {
        [SerializeField] protected List<SplineJoint> joints = new List<SplineJoint>();
        
#if UNITY_EDITOR
        [Header("Editor")]
        [SerializeField] protected bool showSpline = true;
        [SerializeField] protected bool showJoints = true;
        [SerializeField] protected bool showTangents = true;
        [SerializeField] protected Color gizmosColorJoint = Color.green;
        [SerializeField] protected Color gizmosColorTangent = Color.magenta;
        [SerializeField] protected Color gizmosColorLine = Color.red;
        [Range(0.001f, 1f)] [SerializeField] protected float lineAccuracy = 0.05f;
        [Range(0f, 1f)] [SerializeField] protected float jointRadiusAccuracy = 0.1f;
#endif

        public Vector3 Get01(double progress)
        {
            return Get((float)(progress * (joints.Count - 1)));
        }
        public Vector3 Get01(float progress)
        {
            return Get(progress * (joints.Count - 1));
        }
        public Vector3 Get(float progress)
        {
            if (joints.Count == 0)
                return Vector3.zero;
            if (progress <= 0)
                return joints[0].Joint.position;
            if (progress >= joints.Count - 1)
                return joints[joints.Count - 1].Joint.position;

            var id = Mathf.FloorToInt(progress);
            var realProgress = progress - id;
            var l1 = Vector3.Lerp(joints[id].Joint.position, joints[id].EndTangent.position, realProgress);
            var l2 = Vector3.Lerp(joints[id].EndTangent.position, joints[id + 1].StartTangent.position, realProgress);
            var l3 = Vector3.Lerp(joints[id + 1].StartTangent.position, joints[id + 1].Joint.position, realProgress);
            var l4 = Vector3.Lerp(l1, l2, realProgress);
            var l5 = Vector3.Lerp(l2, l3, realProgress);
            var l6 = Vector3.Lerp(l4, l5, realProgress);
            return l6;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var joint in joints)
            {
                if (!joint.GizmosRecalculate())
                {
                    joints.Remove(joint);
                    return;
                }
                if (showJoints)
                {
                    Gizmos.color = gizmosColorJoint;
                    Gizmos.DrawSphere(joint.Joint.position, jointRadiusAccuracy);
                }
                if (showTangents)
                {
                    Gizmos.color = gizmosColorTangent;
                    if (joint.StartTangent != null)
                        Gizmos.DrawSphere(joint.StartTangent.position, jointRadiusAccuracy);
                    if (joint.EndTangent != null)
                        Gizmos.DrawSphere(joint.EndTangent.position, jointRadiusAccuracy);
                }
            }

            if (showSpline)
            {
                Gizmos.color = gizmosColorLine;
                for (float i = 0; i < joints.Count - 1; i += lineAccuracy)
                {
                    Gizmos.DrawLine(Get(i), Get(i + lineAccuracy));
                }
            }
        }
#endif

#if UNITY_EDITOR
        [CustomEditor(typeof(SplineJoints))]
        public class SplineJointsEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                var spliner = target as SplineJoints;
                
                if (spliner == null) return;
                
                if (GUILayout.Button("Add Joint"))
                {
                    if (spliner.joints.Count != 0)
                    {
                        var end = new GameObject("Tangent End").transform;
                        var start = new GameObject("Tangent Start").transform;
                        var obj = new GameObject("Joint").transform;
                        
                        obj.parent = spliner.gameObject.transform;

                        spliner.joints[spliner.joints.Count - 1].SetEnd(end);
                        var endPosition = end.position;
                        var direction = endPosition - spliner.joints[spliner.joints.Count - 1].Joint.position;
                        
                        start.position = endPosition + direction;
                        obj.position = start.position + direction;
                        spliner.joints.Add(new SplineJoint(obj));
                        spliner.joints[spliner.joints.Count - 1].SetStart(start);
                    }
                    else
                    {
                        var obj = new GameObject("Joint").transform;
                        obj.parent = spliner.gameObject.transform;
                        obj.localPosition = Vector3.zero;
                        spliner.joints.Add(new SplineJoint(obj.transform));
                    }
                    
                }
                if (GUILayout.Button("Save Joint"))
                {
                    var path = EditorUtility.SaveFilePanel("Save spline", Application.dataPath, "spline", "asset");
                    
                    if (path == string.Empty)
                    {
                        Debug.LogWarning("Spline asset save path is not selected");
                        return;
                    }
                    
                    var data = CreateInstance<SplineData>();
                    data.Set(spliner.joints);
                    
                    if (data.points.Count == 0)
                    {
                        Debug.LogError("You don't save Spline asset, because it's empty");
                        return;
                    }

                    var indexOf = path.IndexOf("Assets", StringComparison.Ordinal);
                    path = path.Substring(indexOf, path.Length - indexOf);
                    
                    AssetDatabase.CreateAsset(data, path);
                }
                if (GUILayout.Button("Load Joint"))
                {
                    var path = EditorUtility.OpenFilePanel("Load spline", Application.dataPath, "asset");
                    
                    if (path == string.Empty)
                    {
                        Debug.LogWarning("Spline asset is not selected");
                        return;
                    }
                    
                    var indexOf = path.IndexOf("Assets", StringComparison.Ordinal);
                    path = path.Substring(indexOf, path.Length - indexOf);
                    
                    var data = AssetDatabase.LoadAssetAtPath<SplineData>(path);
                    
                    if (data == null)
                    {
                        Debug.LogError("Spline asset is not correct loaded, maybe you choose not SplineData ScriptableObject?");
                        return;
                    }
                    
                    spliner.joints = new List<SplineJoint>();
                    int length = spliner.transform.childCount;
                    for (int i = 0; i < length; i++)
                        DestroyImmediate(spliner.transform.GetChild(0).gameObject);
                    spliner.joints = data.SpawnGet(spliner.transform);
                }
                
                DrawDefaultInspector();
            }
        }
#endif
    }
}
