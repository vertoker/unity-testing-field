using UnityEngine;

namespace CoroutineAnimator.Demo
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private Transform target;
        private Transform _parent;

        public void Awake()
        {
            _parent = transform;
        }

        public void FixedUpdate()
        {
            _parent.localPosition = Vector3.Lerp(_parent.localPosition, target.localPosition, smoothTime);
        }
    }
}
