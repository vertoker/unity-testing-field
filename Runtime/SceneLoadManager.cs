using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

namespace Game.WorldGeneration
{
    public class SceneLoadManager : DataInitialize
    {
        [SerializeField] private string _sceneName;
        private IntDelegate _sizeXDelegate, _sizeYDelegate;
        private Dictionary<int, Scene> _scenes = new Dictionary<int, Scene>();
        
        private static UnityEvent<int> _sceneLoaded = new UnityEvent<int>();
        private static UnityEvent<int> _sceneUnloaded = new UnityEvent<int>();

        public static event UnityAction<int> SceneLoaded
        {
            add => _sceneLoaded.AddListener(value);
            remove => _sceneLoaded.RemoveListener(value);
        }
        public static event UnityAction<int> SceneUnloaded
        {
            add => _sceneUnloaded.AddListener(value);
            remove => _sceneUnloaded.RemoveListener(value);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += LoadedScene;
            SceneManager.sceneUnloaded += UnloadedScene;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= LoadedScene;
            SceneManager.sceneUnloaded -= UnloadedScene;
        }
        public override void Initialize(IntDelegate chunksLoadCountXDelegate, IntDelegate chunksLoadCountYDelegate, IntDelegate sizeXDelegate, IntDelegate sizeYDelegate)
        {
            _sizeXDelegate = sizeXDelegate;
            _sizeYDelegate = sizeYDelegate;
        }

        public void Load(int posX, int posY)
        {
            ChunkContext context = new ChunkContext(_sizeXDelegate.Invoke(), _sizeYDelegate.Invoke(), posX, posY);
            ChunkDataTransfer.SetContext(context);
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
        }
        public void LoadAsync(Vector2Int pos)
        {
            ChunkContext context = new ChunkContext(_sizeXDelegate.Invoke(), _sizeYDelegate.Invoke(), pos.x, pos.y);
            ChunkDataTransfer.SetContext(context);
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        }

        [System.Obsolete]
        public void Unload(int hashCode)
        {
            SceneManager.UnloadScene(_scenes[hashCode]);
        }
        public void UnloadAsync(int hashCode)
        {
            SceneManager.UnloadSceneAsync(_scenes[hashCode]);
        }

        private void LoadedScene(Scene scene, LoadSceneMode sceneMode)
        {
            if (sceneMode == LoadSceneMode.Additive)
            {
                _scenes.Add(scene.GetHashCode(), scene);
                _sceneLoaded.Invoke(scene.GetHashCode());
            }
#if UNITY_EDITOR
            Debug.Log(string.Format("Scene {0} is loaded. Current scenes count: {1}", scene.GetHashCode(), SceneManager.sceneCount));
#endif
        }
        private void UnloadedScene(Scene scene)
        {
            _scenes.Remove(scene.GetHashCode());
            _sceneUnloaded.Invoke(scene.GetHashCode());
#if UNITY_EDITOR
            Debug.Log(string.Format("Scene {0} is unloaded. Current scenes count: {1}", scene.GetHashCode(), SceneManager.sceneCount));
#endif
        }
    }
}