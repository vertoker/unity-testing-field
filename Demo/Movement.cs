using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Demo
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _rb.velocity = new Vector2(horizontal, vertical) * _speed;
        }
    }
}