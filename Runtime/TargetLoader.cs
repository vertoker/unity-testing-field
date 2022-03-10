using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldGeneration
{
    [RequireComponent(typeof(SceneLoadManager))]
    public class TargetLoader : DataInitialize
    {
        [SerializeField] private Transform _target;
        [SerializeField] private SceneLoadManager _loadManager;
        private IntDelegate _chunksLoadCountXDelegate;
        private IntDelegate _chunksLoadCountYDelegate;
        private IntDelegate _sizeXDelegate, _sizeYDelegate;
        private Vector2Int _current;

        private Dictionary<Vector2Int, int> _scenesLocation = new Dictionary<Vector2Int, int>();

        private void OnEnable()
        {
            ChunkDataTransfer.SceneGetContext += Add;
        }
        private void OnDisable()
        {
            ChunkDataTransfer.SceneGetContext -= Add;
        }
        private void Start()
        {
            InitializeUpdateChunks();
        }

        private void Add(int hashCode, ChunkContext context)
        {
            Vector2Int position = new Vector2Int(context.PosX, context.PosY);
            _scenesLocation.Add(position, hashCode);
        }

        public override void Initialize(IntDelegate chunksLoadCountXDelegate, IntDelegate chunksLoadCountYDelegate, IntDelegate sizeXDelegate, IntDelegate sizeYDelegate)
        {
            _chunksLoadCountXDelegate = chunksLoadCountXDelegate;
            _chunksLoadCountYDelegate = chunksLoadCountYDelegate;
            _sizeXDelegate = sizeXDelegate;
            _sizeYDelegate = sizeYDelegate;
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
                Vector2Int delta = now - _current;
                int chunksCountX = _chunksLoadCountXDelegate.Invoke();
                int chunksCountY = _chunksLoadCountYDelegate.Invoke();
                //Добавить учёт размера чанков
                Vector2Int[] deleteChunks = PosEnvironment.GetDeleteOffset(delta);
                Vector2Int[] spawnChunks = PosEnvironment.GetSpawnOffset(delta);

                foreach (var chunkCoord in deleteChunks)
                {
                    int hashCode = _scenesLocation[_current + chunkCoord];
                    _loadManager.UnloadAsync(hashCode);
                    _scenesLocation.Remove(_current + chunkCoord);
                }
                foreach (var chunkCoord in spawnChunks)
                {
                    _loadManager.LoadAsync(now + chunkCoord);
                }

                _current = now;
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