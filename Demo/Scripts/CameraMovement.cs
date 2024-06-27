using UnityEngine;

namespace CoroutineAnimator.Demo
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private Transform target;
        private Transform parent;

        public void Awake()
        {
            parent = transform;
        }

        public void FixedUpdate()
        {
            parent.localPosition = Vector3.Lerp(parent.localPosition, target.localPosition, smoothTime);
        }
    }
}
