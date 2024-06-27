using UnityEngine;

namespace CoroutineAnimator.Demo
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 0.25f;
        [SerializeField] private AnimationData[] dataIdle;
        [SerializeField] private AnimationData[] dataRun;
        
        private int _direction;
        private bool _move;
        private Transform _parent;
        private SpriteAnimation _animator;

        private void Start()
        {
            _parent = transform;
            _animator = GetComponent<SpriteAnimation>();
            AnimUpdate();
        }
        private void FixedUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            
            var translation = new Vector3(horizontal, vertical, 0) * speed;
            _parent.Translate(translation);

            var nextDirection = _direction;
            var nextMove  = translation.sqrMagnitude != 0f;
            if (nextMove) nextDirection = DirectionToIndex(translation, 8);
            if (_direction == nextDirection && _move == nextMove) return;
            
            _direction = nextDirection;
            _move = nextMove;
            AnimUpdate();
        }
        private void AnimUpdate()
        {
            _animator.Data = _move ? dataRun[_direction] : dataIdle[_direction];
            _animator.Play = true;
        }
        
        private int DirectionToIndex(Vector2 dir, int sliceCount)
        {
            var normDir = dir.normalized;
            var step = 360f / sliceCount;
            var angle = Vector2.SignedAngle(Vector2.up, normDir) + step / 2;
            if (angle < 0) { angle += 360; }
            return Mathf.FloorToInt(angle / step);
        }
    }
}
