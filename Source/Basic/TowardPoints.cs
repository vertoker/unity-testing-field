using System.Collections.Generic;
using UnityEngine;

namespace InverseKinematic.Basic
{
    public class TowardPoints : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private Transform target;
        [SerializeField] private List<Vector3> points;
        
        private int _currentTarget;
        
        public void Update()
        {
            target.position = Vector3.MoveTowards(target.position, points[_currentTarget], speed * Time.deltaTime);

            if (target.position != points[_currentTarget]) return;
            
            _currentTarget++;
            if (points.Count == _currentTarget)
                _currentTarget = 0;
        }
    }
}
