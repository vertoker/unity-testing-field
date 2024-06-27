using UnityEngine;

namespace InverseKinematic
{
    public class BonesIK : MonoBehaviour
    {
        [SerializeField] protected Transform target;
        [SerializeField] protected Transform originBone;

        [Header("Animation Parameters")]
        [SerializeField] protected int iterations = 10;
        [SerializeField] protected float delta = 0.001f;

        private Transform[] _joints;
        private Transform[] _bones;
        private Vector3[] _bonesScales;
        private float[] _bonesLength;
        private Vector3[] _positions;
        private int _length;
        
        private float _fullLength;
        
        public void Awake()
        {
            _fullLength = 0;
            var self = transform;
            _length = self.childCount - 1;
            _bonesLength = new float[_length];
            _bonesScales = new Vector3[_length];
            _positions = new Vector3[_length + 1];
            _joints = new Transform[_length + 1]; // Inverted
            _bones = new Transform[_length]; // Inverted
            
            _joints[_length] = self.GetChild(0);
            for (var i = _length - 1; i >= 0; i--)
            {
                _joints[i] = self.GetChild(_length - i);
                _bones[i] = Instantiate(originBone, self);
                
                _bonesLength[i] = (_joints[i].position - _joints[i + 1].position).magnitude;
                _bonesScales[i] = new Vector3(_bones[i].localScale.x, _bones[i].localScale.y, _bonesLength[i]);
                _fullLength += _bonesLength[i];
            }
            
            LateUpdate();

            for (var i = 0; i < _length; i++)
                _bones[i].gameObject.SetActive(true);
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

            for (var i = 0; i < _length; i++)
            {
                _bones[i].position = (_positions[i] + _positions[i + 1]) / 2;
                _bones[i].LookAt(_positions[i]);
                _bones[i].localScale = _bonesScales[i];
            }
            
            for (var i = 0; i <= _length; i++)
                _joints[i].position = _positions[i];
        }
    }
}
