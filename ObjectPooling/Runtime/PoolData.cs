using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    [CreateAssetMenu(menuName = "Pool/New Pool Data", fileName = "Pool Data", order = 0)]
    public class PoolData : ScriptableObject
    {
        [SerializeField] private GameObject _object;
        [SerializeField] private int _startCapacity;

        public GameObject GetObject => _object;
        public int GetCapacity => _startCapacity;
    }
}