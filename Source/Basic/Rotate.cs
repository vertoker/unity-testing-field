using UnityEngine;

namespace InverseKinematic.Basic
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private Vector3 speed = new Vector3(0, 0, 0);
        [SerializeField] private Space spaceType = Space.Self;
        
        public void Update()
        {
            transform.Rotate(speed * Time.deltaTime, spaceType);
        }
    }
}
