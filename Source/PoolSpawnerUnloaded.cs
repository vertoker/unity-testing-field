using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ObjectPooling
{
    public class PoolSpawnerUnloaded : MonoBehaviour
    {
        [SerializeField] private PoolData _data;
        [SerializeField] private Queue<GameObject> _pool = new Queue<GameObject>();
        [SerializeField] private bool _spawnInParent = false;
        [ShowIf(ActionOnConditionFail.Disable, ConditionOperator.And, nameof(_spawnInParent))]
        [SerializeField] private Transform parent;
        private readonly UnityEvent _onLoad = new UnityEvent();

        private void Start()
        {
            if (_spawnInParent)
            {
                if (parent == null)
                    parent = transform;
            }
            SceneManager.activeSceneChanged += EnqueueAll;
            DontDestroyOnLoad(gameObject);
            if (_data.GetObject.activeSelf)
                _data.GetObject.SetActive(false);
            for (int i = 0; i < _data.GetCapacity; i++)
                _pool.Enqueue(CreateItem());
        }

        /// <summary>
        /// Return object (don't forget return back this)
        /// </summary>
        /// <param name="activateItem">Set active object or not</param>
        /// <returns></returns>
        public GameObject Dequeue(bool activateItem = true)
        {
            if (_pool.Count == 0)
                _pool.Enqueue(CreateItem());
            GameObject item = _pool.Dequeue();
            if (activateItem)
                item.SetActive(true);
            return item;
        }
        /// <summary>
        /// Return object and return it back after a while
        /// </summary>
        /// <param name="time">After this time in seconds object return back</param>
        /// <param name="activateItem">Set active object or not</param>
        /// <returns></returns>
        public GameObject Dequeue(float time, bool activateItem = true)
        {
            GameObject item = Dequeue(activateItem);
            StartCoroutine(DequeueDestroy(item, time));
            return item;
        }
        /// <summary>
        /// Return back object
        /// </summary>
        /// <param name="item">Item, which returned</param>
        public void Enqueue(GameObject item)
        {
            item.SetActive(false);
            _pool.Enqueue(item);
        }

        private void EnqueueAll(Scene past, Scene next)
        {
            _onLoad.Invoke();
        }

        private UnityAction GetDisableOnLoad(GameObject item)
        {
            return new UnityAction(() =>
            {
                if (!_pool.Contains(item))
                    Enqueue(item);
            });
        }

        private GameObject CreateItem()
        {
            GameObject item = Instantiate(_data.GetObject);
            _onLoad.AddListener(GetDisableOnLoad(item));
            DontDestroyOnLoad(item);
            if (_spawnInParent)
                return Instantiate(item, parent);
            return Instantiate(item);
        }
        private IEnumerator DequeueDestroy(GameObject item, float time)
        {
            yield return new WaitForSeconds(time);
            Enqueue(item);
        }
    }
}