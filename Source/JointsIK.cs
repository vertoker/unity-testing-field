using UnityEngine;

namespace InverseKinematic
{
    public class JointsIK : MonoBehaviour
    {
        [SerializeField] protected Transform target;

        [Header("Animation Parameters")]
        [SerializeField] protected int iterations = 10;
        [SerializeField] protected float delta = 0.001f;
        
        private Transform[] _joints;
        private float[] _bonesLength;
        private Vector3[] _positions;
        private int _length;
        
        private float _fullLength;
        
        public void Awake()
        {
            _fullLength = 0;
            _length = transform.childCount - 1;
            _bonesLength = new float[_length];
            _positions = new Vector3[_length + 1];
            _joints = new Transform[_length + 1]; // Inverted

            _joints[_length] = transform.GetChild(0);
            for (var i = _length - 1; i >= 0; i--)
            {
                _joints[i] = transform.GetChild(_length - i);
                _bonesLength[i] = (_joints[i].position - _joints[i + 1].position).magnitude;
                _fullLength += _bonesLength[i];
            }
        }
        public void LateUpdate()
        {
            for (var i = 0; i <= _length; i++)
                _positions[i] = _joints[i].position;

            var targetPosition = target.position;
            if ((targetPosition - _joints[0].position).sqrMagnitude >= _fullLength * _fullLength)
            {
                var direction = (targetPosition - _positions[0]).normalized;
                
                for (var i = 1; i <= _length; i++)
                {
                    _positions[i] = _positions[i - 1] + direction * _bonesLength[i - 1];
                }
            }
            else
            {
                for (var iteration = 0; iteration < iterations; iteration++)
                {
                    _positions[_length] = targetPosition;
                    
                    for (var i = _length - 1; i > 0; i--)
                    {
                        var direction = (_positions[i] - _positions[i + 1]).normalized;
                        _positions[i] = _positions[i + 1] + direction * _bonesLength[i];
                    }
                    for (var i = 1; i <= _length; i++)
                    {
                        var direction = (_positions[i] - _positions[i - 1]).normalized;
                        _positions[i] = _positions[i - 1] + direction * _bonesLength[i - 1];
                    }
                    
                    if ((targetPosition - _positions[_length]).sqrMagnitude >= delta * delta)
                        break;
                }
            }
            
            for (var i = 0; i <= _length; i++)
                _joints[i].position = _positions[i];
        }
    }
}
