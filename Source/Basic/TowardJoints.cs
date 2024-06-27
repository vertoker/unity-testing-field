using System.Collections.Generic;
using UnityEngine;

namespace InverseKinematic.Basic
{
    public class TowardJoints : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private Transform target;
        [SerializeField] private List<Transform> joints;
        
        private int _currentTarget;
        
        public void Update()
        {
            target.position = Vector3.MoveTowards(target.position, joints[_currentTarget].position, speed * Time.deltaTime);

            if (target.position != joints[_currentTarget].position) return;
            
            _currentTarget++;
            if (joints.Count == _currentTarget)
                _currentTarget = 0;
        }
    }
}