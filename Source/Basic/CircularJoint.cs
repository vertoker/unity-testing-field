using UnityEngine;

namespace InverseKinematic.Basic
{
    public class CircularJoint : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private Transform joint;
        [SerializeField] private Transform center;
        [SerializeField] private Transform axis;

        public void Update()
        {
            joint.RotateAround(center.position, axis.localPosition, speed * Time.deltaTime);
        }
    }
}
