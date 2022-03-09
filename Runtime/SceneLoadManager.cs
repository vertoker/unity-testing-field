using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Game.WorldGeneration
{
    public class SceneLoadManager : DataInitialize
    {
        [SerializeField] private string _sceneName;
        private IntDelegate _sizeXDelegate, _sizeYDelegate;

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
        public void LoadAsync(int posX, int posY)
        {
            ChunkContext context = new ChunkContext(_sizeXDelegate.Invoke(), _sizeYDelegate.Invoke(), posX, posY);
            ChunkDataTransfer.SetContext(context);
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        }
        public void Unload(int posX, int posY)
        {

        }

        private void LoadedScene(Scene scene, LoadSceneMode sceneMode)
        {
            Debug.Log(string.Format("Scene {0} is loaded. Current scenes count: {1}", scene.GetHashCode(), SceneManager.sceneCount));
        }
        private void UnloadedScene(Scene scene)
        {

        }
    }
}