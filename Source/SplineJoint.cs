using UnityEngine;

namespace Splines
{
    [System.Serializable]
    public class SplineJoint
    {
        [SerializeField] private TangentType type;
        [SerializeField] private Transform startTangent;
        [SerializeField] private Transform joint;
        [SerializeField] private Transform endTangent;
        private bool containsBothTangents = false;

        public TangentType Type => type;
        public Transform StartTangent => startTangent;
        public Transform Joint => joint;
        public Transform EndTangent => endTangent;
        
#if UNITY_EDITOR
        private Vector3 startTangentPos;
        private Vector3 jointPos;
        private Vector3 endTangentPos;
#endif
        
#if UNITY_EDITOR
        public bool GizmosRecalculate()
        {
            if (joint == null)
                return false;
            if (jointPos != joint.position)
            {
                jointPos = joint.position;
                if (startTangent)
                    startTangentPos = startTangent.position;
                if (endTangent)
                    endTangentPos = endTangent.position;
            }
            else if (containsBothTangents)
            {
                if (startTangentPos != startTangent.position)
                {
                    RecalculateEnd();
                    startTangentPos = startTangent.position;
                }
                else if (endTangentPos != endTangent.position)
                {
                    RecalculateStart();
                    endTangentPos = endTangent.position;
                }
            }
            return true;
        }
#endif

        public SplineJoint(Transform joint)
        {
            this.joint = joint;
            type = TangentType.Mirrored;
            
#if UNITY_EDITOR
            jointPos = joint.position;
#endif
        }
        public void SetStart(Transform start)
        {
            startTangent = start;
            startTangent.parent = joint;
            
            containsBothTangents = startTangent && endTangent;
            RecalculateStart();
            
#if UNITY_EDITOR
            startTangentPos = startTangent.position;
#endif
        }
        public void SetEnd(Transform end)
        {
            endTangent = end;
            endTangent.parent = joint;
            
            containsBothTangents = startTangent && endTangent;
            RecalculateEnd();
            
#if UNITY_EDITOR
            if (startTangent == null)
                endTangent.position = joint.position + joint.forward;
            endTangentPos = endTangent.position;
#endif
        }
        public void SetType(TangentType type)
        {
            this.type = type;
        }

        /// <summary>
        /// Calls when endTangent changing
        /// </summary>
        public void RecalculateStart()
        {
            if (!containsBothTangents) return;
            
            if (type == TangentType.Mirrored)
            {
                var jointPosition = joint.position;
                var direction = jointPosition - endTangent.position;
                startTangent.position = jointPosition + direction;
            }
            else if (type == TangentType.Aligned)
            {
                var jointPosition = joint.position;
                var startDistance = (jointPosition - startTangent.position).magnitude;
                var direction = (jointPosition - endTangent.position).normalized;
                startTangent.position = jointPosition + direction * startDistance;
            }
            
#if UNITY_EDITOR
            startTangentPos = startTangent.position;
#endif
        }
        /// <summary>
        /// Calls when startTangent changing
        /// </summary>
        public void RecalculateEnd()
        {
            if (!containsBothTangents) return;

            if (type == TangentType.Mirrored)
            {
                var jointPosition = joint.position;
                var direction = jointPosition - startTangent.position;
                endTangent.position = jointPosition + direction;
            }
            else if (type == TangentType.Aligned)
            {
                var jointPosition = joint.position;
                var endDistance = (jointPosition - endTangent.position).magnitude;
                var direction = (jointPosition - startTangent.position).normalized;
                endTangent.position = jointPosition + direction * endDistance;
            }
            
#if UNITY_EDITOR
            endTangentPos = endTangent.position;
#endif
        }
    }
}
