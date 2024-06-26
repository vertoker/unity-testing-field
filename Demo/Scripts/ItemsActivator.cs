using System.Collections.Generic;
using UnityEngine;

namespace SpatialHashing.Demo
{
    public class ItemsActivator : MonoBehaviour
    {
        [SerializeField] private float radius = 3f;
        [SerializeField] private ItemsSpawner spawner;

        private readonly List<SpatialHash<Item>.Hit> _hits =
            new List<SpatialHash<Item>.Hit>();

        [Header("Debug")]
        [SerializeField] private int hitsCount;
        
        private void Update()
        {
            foreach (var hit in _hits)
                hit.Item.SetDisable();

            var pos = transform.position;
            spawner.Items.Get(pos.x, pos.y, pos.z, radius, false, _hits);
            
            foreach (var hit in _hits)
                hit.Item.SetEnable();
            hitsCount = _hits.Count;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}