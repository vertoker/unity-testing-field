using UnityEngine;

namespace SpatialHashing.Demo
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Material enableMaterial;
        [SerializeField] private Material disableMaterial;
        [SerializeField] private new MeshRenderer renderer;

        public Vector3 Position => transform.position;

        public void SetEnable()
        {
            renderer.material = enableMaterial;
        }
        public void SetDisable()
        {
            renderer.material = disableMaterial;
        }
    }
}