using UnityEngine;

namespace CustAnim.Basic
{
    public class CircularJointUpdate : MonoBehaviour
    {
        [SerializeField] protected Transform joint;
        [SerializeField] protected Transform center;
        [SerializeField] protected Transform axis;
        [Space]
        [SerializeField] protected float speed = 1f;

        public void Update()
        {
            joint.RotateAround(center.position, axis.localPosition, speed * Time.deltaTime);
        }
    }
}
