using System.Collections.Generic;
using UnityEngine;

namespace Splines
{
    public class SplineData : ScriptableObject
    {
        public List<Vector3> points;
        public List<TangentType> types;

        public void Set(List<SplineJoint> joints)
        {
            var count = joints.Count;
            points = new List<Vector3>();
            types = new List<TangentType>();
            if (count == 0) return;
            
            points.Add(joints[0].Joint.position);
            types.Add(joints[0].Type);
            if (count == 1) return;

            count--;
            points.Add(joints[0].EndTangent.position);
            for (int i = 1; i < count; i++)
            {
                points.Add(joints[i].StartTangent.position);
                points.Add(joints[i].Joint.position);
                points.Add(joints[i].EndTangent.position);
                types.Add(joints[i].Type);
            }
            points.Add(joints[count].StartTangent.position);
            points.Add(joints[count].Joint.position);
            types.Add(joints[count].Type);
        }

        public List<SplineJoint> SpawnGet(Transform parent)
        {
            var joints = new List<SplineJoint>();
            if (points.Count == 0)
                return joints;

            var counter = 0;
            var obj = new GameObject("Joint").transform;
            obj.parent = parent;
            joints.Add(new SplineJoint(obj));
            joints[0].SetType(types[0]);
            obj.position = points[counter++];
            if (points.Count == 1)
                return joints;
            
            var end = new GameObject("Tangent End").transform;
            end.parent = obj;
            joints[0].SetEnd(end);
            end.position = points[counter++];

            var length = (points.Count - 1) / 3;
            Transform start;
            for (int i = 1; i < length; i++)
            {
                start = new GameObject("Tangent Start").transform;
                obj = new GameObject("Joint").transform;
                end = new GameObject("Tangent End").transform;
                obj.parent = parent;

                start.position = points[counter++];
                obj.position = points[counter++];
                end.position = points[counter++];
                
                joints.Add(new SplineJoint(obj));
                joints[i].SetType(types[i]);
                joints[i].SetStart(start);
                joints[i].SetEnd(end);
            }
            start = new GameObject("Tangent Start").transform;
            obj = new GameObject("Joint").transform;
            obj.parent = parent;
            
            start.position = points[counter++];
            obj.position = points[counter++];
            
            joints.Add(new SplineJoint(obj));
            joints[joints.Count - 1].SetType(types[types.Count - 1]);
            joints[joints.Count - 1].SetStart(start);
            return joints;
        }
    }
}
