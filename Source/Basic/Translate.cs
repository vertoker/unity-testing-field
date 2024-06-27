using UnityEngine;

namespace InverseKinematic.Basic
{
    public class Translate : MonoBehaviour
    {
        [SerializeField] protected Vector3 speed = new Vector3(0, 0, 0);
        [SerializeField] protected Space spaceType = Space.Self;
        
        public void Update()
        {
            transform.Translate(speed * Time.deltaTime, spaceType);
        }
    }
}
