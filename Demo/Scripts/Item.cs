using UnityEngine;

namespace SpatialHashing.Demo
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Material green;
        [SerializeField] private Material red;
        [SerializeField] private new MeshRenderer renderer;

        public Vector3 Position => transform.position;

        public void SetEnable()
        {
            renderer.material = green;
        }
        public void SetDisable()
        {
            renderer.material = red;
        }
    }
}