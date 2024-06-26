using UnityEngine;

namespace SpatialHashing.Demo
{
    public class ItemsSpawner : MonoBehaviour
    {
        [SerializeField] private Item source;
        [SerializeField] private int objectsCount = 1000;
        private Item[] _objects;
        [Space]
        [SerializeField] private int cellSize = 2;
        [SerializeField] private Vector3 min = new Vector3(-20, -20, -20);
        [SerializeField] private Vector3 max = new Vector3(20, 20, 20);
        public SpatialHash<Item> Items { get; private set; }

        private void Start()
        {
            Items = new SpatialHash<Item>(cellSize, 
                min.x, min.y, min.z, max.x, max.y, max.z);
            _objects = new Item[objectsCount];
            
            for (var i = 0; i < objectsCount; i++)
            {
                var pos = GetRandomPos();
                var rot = GetRandomRot();
                _objects[i] = Instantiate(source, pos, rot, transform);
                
                _objects[i].SetDisable();
                Items.Add(_objects[i], pos.x, pos.y, pos.z);
            }
        }

        private Vector3 GetRandomPos()
        {
            var x = Random.Range(min.x, max.x);
            var y = Random.Range(min.y, max.y);
            var z = Random.Range(min.z, max.z);
            return new Vector3(x, y, z);
        }
        private Quaternion GetRandomRot()
        {
            var axis = new Vector3(Random.value, Random.value, Random.value);
            axis.Normalize();
            return new Quaternion(axis.x, axis.y, axis.z, Random.value);
        }
        
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            var x = max.x - min.x;
            var y = max.y - min.y;
            var z = max.z - min.z;
            var size = new Vector3(x, y, z);
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}
