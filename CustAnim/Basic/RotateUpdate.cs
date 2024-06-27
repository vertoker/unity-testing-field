using System;
using CustAnim.Editor;
using UnityEngine;

namespace CustAnim.Basic
{
    public class RotateUpdate : MonoBehaviour
    {
        [SerializeField] protected Vector3 speed = new Vector3(0, 0, 0);
        [SerializeField] protected Space spaceType = Space.Self;
        [Space]
        [SerializeField] protected bool initializeOnAwake = true;
        [Show(ActionOnConditionFail.DoNotDraw, InverseCondition.No, "initializeOnAwake")]
        [SerializeField] protected Transform self;

        private void Awake()
        {
            if (initializeOnAwake)
                Initialize();
        }
        public void Initialize()
        {
            self = transform;
        }

        public void Update()
        {
            self.Rotate(speed, spaceType);
        }
    }
}
