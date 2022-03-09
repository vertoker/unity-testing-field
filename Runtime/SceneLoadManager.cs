using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Game.WorldGeneration
{
    public class SceneLoadManager : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private int _sizeX, _sizeY;

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
        public void UpdateChunks()
        {
            Load(0, 0);
        }

        private void Load(int posX, int posY)
        {
            ChunkContext context = new ChunkContext(_sizeX, _sizeY, posX, posY);
            ChunkDataTransfer.SetContext(context);
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        }
        private void Unload(long sceneID)
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