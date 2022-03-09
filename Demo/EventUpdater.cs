using System.Collections;
using UnityEngine;
using Game.WorldGeneration;

namespace Game.Demo
{
    public class EventUpdater : MonoBehaviour
    {
        [SerializeField] private float _time = 0.5f;
        private SceneLoadManager _manager;
        private Coroutine _main;

        private void Start()
        {
            _manager = GetComponent<SceneLoadManager>();
            _main = StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            int counter = 10;
            while (counter > 0)
            {
                yield return new WaitForSeconds(_time);
                _manager.UpdateChunks();
                counter--;
            }
        }
    }
}