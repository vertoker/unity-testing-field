using UnityEngine;

namespace ObjectPooling
{
    [CreateAssetMenu(menuName = "Pool/New Pool Data", fileName = "Pool Data", order = 0)]
    public class PoolData : ScriptableObject
    {
        [Tooltip("Object, which will be spawn")]
        [SerializeField] private GameObject _object;
        [Tooltip("How many objects will be spawn at start")]
        [SerializeField] private int _startCapacity;

        public GameObject GetObject => _object;
        public int GetCapacity => _startCapacity;
    }
}