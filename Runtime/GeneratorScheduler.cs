using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldGeneration
{
    [RequireComponent(typeof(SceneLoadManager))]
    public class GeneratorScheduler : MonoBehaviour
    {
        [SerializeField] private int _chunksLoadCountX = 1;
        [SerializeField] private int _chunksLoadCountY = 1;
        [SerializeField] private int _sizeX = 16, _sizeY = 16;

        [SerializeField] private SceneLoadManager _loadManager;
        [SerializeField] private DataInitialize _loader;
        
        private IntDelegate _chunksLoadCountXDelegate;
        private IntDelegate _chunksLoadCountYDelegate;
        private IntDelegate _sizeXDelegate, _sizeYDelegate;

        private void Awake()
        {
            _chunksLoadCountXDelegate = () => { return _chunksLoadCountX; };
            _chunksLoadCountYDelegate = () => { return _chunksLoadCountY; };

            _sizeXDelegate = () => { return _sizeX; };
            _sizeYDelegate = () => { return _sizeY; };

            _loadManager.Initialize(_chunksLoadCountXDelegate, _chunksLoadCountYDelegate, _sizeXDelegate, _sizeYDelegate);
            _loader.Initialize(_chunksLoadCountXDelegate, _chunksLoadCountYDelegate, _sizeXDelegate, _sizeYDelegate);
        }
    }

    public delegate float FloatDelegate();
    public delegate int IntDelegate();
}