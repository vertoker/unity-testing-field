using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldGeneration
{
    [RequireComponent(typeof(SceneLoadManager))]
    public class TargetLoader : DataInitialize
    {
        [SerializeField] private Transform _target;
        private SceneLoadManager _loadManager;
        private IntDelegate _chunksLoadCountXDelegate;
        private IntDelegate _chunksLoadCountYDelegate;
        private IntDelegate _sizeXDelegate, _sizeYDelegate;

        private Vector2Int _current;

        private void OnEnable()
        {
            _loadManager = GetComponent<SceneLoadManager>();
        }

        public override void Initialize(IntDelegate chunksLoadCountXDelegate, IntDelegate chunksLoadCountYDelegate, IntDelegate sizeXDelegate, IntDelegate sizeYDelegate)
        {
            _chunksLoadCountXDelegate = chunksLoadCountXDelegate;
            _chunksLoadCountYDelegate = chunksLoadCountYDelegate;
            _sizeXDelegate = sizeXDelegate;
            _sizeYDelegate = sizeYDelegate;
        }

        private void Start()
        {
            InitializeUpdateChunks();
        }

        public void InitializeUpdateChunks()
        {
            _current = Position2Grid(_target.position);
            int chunksCountX = _chunksLoadCountXDelegate.Invoke();
            int chunksCountY = _chunksLoadCountYDelegate.Invoke();

            for (int x = _current.x - chunksCountX; x <= _current.x + chunksCountX; x++)
            {
                for (int y = _current.y - chunksCountY; y <= _current.y + chunksCountY; y++)
                {
                    _loadManager.Load(x, y);
                }
            }
        }
        public void UpdateChunks()
        {
            Vector2Int now = Position2Grid(_target.position);
            if (now != _current)
            {
                _current = now;
                int chunksCountX = _chunksLoadCountXDelegate.Invoke();
                int chunksCountY = _chunksLoadCountYDelegate.Invoke();
                //Работа с тем, какие сцены нуждаются в выгрузке
                //Работа с тем, какие сцены нуждаются в загрузке
            }
        }
        private bool ComputateDistance(Vector2Int chunkCoord)
        {
            return false;
        }

        private Vector2Int Position2Grid(Vector3 position)
        {
            int sizeX = _sizeXDelegate.Invoke(), sizeY = _sizeYDelegate.Invoke();
            int x = (int)Mathf.Round((position.x) / sizeX);
            int y = (int)Mathf.Round((position.y) / sizeY);
            
            //Debug.Log(string.Join(" ", x, y, position, sizeX, sizeY));
            return new Vector2Int(x, y);
        }
    }
}