using System.Collections;
using System.Collections.Generic;
using MapSystem2D.Spawn;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace MapSystem2D
{
    [RequireComponent(typeof(Grid))]
    public class Map : MonoBehaviour
    {
        #region Params
        [SerializeField] private Transform _target;

        [Header("Map Settings")]
        [SerializeField] private bool _initOnAwake = true;
        private bool _inited = false;
        [SerializeField] private bool _generateTerrain = true;
        [SerializeField] private bool _generateObjects = true;
        [SerializeField] private bool _generateCollider = true;
        [Space]
        [SerializeField] private int _seed = 1337;
        [SerializeField] private int _halfSizeMapWidth = 250000;
        [SerializeField] private int _halfSizeMapHeight = 250000;
        [SerializeField] private int _sizeColliderWidth = 10;
        [SerializeField] private int _sizeColliderHeight = 10;
        [SerializeField] private int _sizeViewWidth = 21;
        [SerializeField] private int _sizeViewHeight = 21;

        private static Vector2 _sizeGrid = new Vector2(1f, 1f);
        private static Noise _noise;// Natural map generator
        private Coroutine update;

        [Header("Blocks")]
        [SerializeField] private Blocks _blocks;
        public static Blocks Blocks;

        [Header("Terrain")]
        [ShowIf(ActionOnConditionFail.Disable, ConditionOperator.And, nameof(_generateTerrain))] 
        [SerializeField] private Tilemap _background;
        [ShowIf(ActionOnConditionFail.Disable, ConditionOperator.And, nameof(_generateTerrain))] 
        [SerializeField] private Tilemap _walls;
        [ShowIf(ActionOnConditionFail.Disable, ConditionOperator.And, nameof(_generateTerrain))] 

        private float _timeColliderUpdate = 0.1f;
        private int _countTilesInUpdate = 1500;
        private Vector3Int _current;
        private Vector2Int _offset, _movement;
        private bool _environmentInProcess = false;
        private int _lengthOfEnvironment = 0;
        private int _counter = 0;

        [Space(15)]
        [ShowIf(ActionOnConditionFail.Disable, ConditionOperator.And, nameof(_generateObjects))]
        [SerializeField] private ObjectMapData[] _objects;
        private int _lengthObjects;
        public static Noise Noise => _noise;

        [Header("Collider")]
        [ShowIf(ActionOnConditionFail.Disable, ConditionOperator.And, nameof(_generateCollider))]
        [SerializeField] private GameObject _boxOriginal;
        [ShowIf(ActionOnConditionFail.Disable, ConditionOperator.And, nameof(_generateCollider))]
        [SerializeField] private Transform _colliderParent;
        private Queue<GameObject> _colliderPool;
        private Coroutine coroutineCollider;
        #endregion

        #region Body Public
        /// <summary>
        /// Initialize map generator
        /// </summary>
        public void Init()
        {
            _sizeGrid = GetComponent<Grid>().cellSize;
            _lengthObjects = _objects.Length;
            _noise = new Noise(_seed);
            Blocks = _blocks;

            //Start reset environment generation
            _current = Pos2Grid(_target.localPosition);
            _offset = new Vector2Int(_sizeViewWidth, _sizeViewHeight);
            _lengthOfEnvironment = (_offset.x * 2 + 1) * (_offset.y * 2 + 1);

            //Init blocks
            for (int i = 0; i < _lengthObjects; i++)
                _objects[i].Init();

            //Start generation of environment
            for (int x = _current.x - _offset.x; x < _current.x + _offset.x; x++)
            {
                for (int y = _current.y - _offset.y; y < _current.y + _offset.y; y++)
                {
                    Generate(new Vector3Int(x, y, 0));
                }
            }

            //Create collider environment queue
            if (_colliderPool == null)
            {
                _colliderPool = new Queue<GameObject>();
                _boxOriginal.GetComponent<BoxCollider2D>().size = _sizeGrid;
                int length = (_sizeColliderWidth * 2 + 1) * (_sizeColliderHeight * 2 + 1);
                for (int i = 0; i < length; i++)
                    _colliderPool.Enqueue(Instantiate(_boxOriginal, _colliderParent));//Added object in queue
            }

            _inited = true;

            if (coroutineCollider != null)
                StopCoroutine(coroutineCollider);
            coroutineCollider = StartCoroutine(UpdateCollider());

            if (update != null)
                StopCoroutine(update);
            update = StartCoroutine(UpdateTerrain());
        }
        /// <summary>
        /// Update view parameters (use when camera change size)
        /// </summary>
        /// <param name="camera">Using to understand how far camera see</param>
        public void ViewUpdate(Camera camera)
        {
            Vector2 scale = camera.transform.lossyScale;
            Vector2 view = new Vector2(scale.x * camera.aspect, scale.y) * camera.orthographicSize;
            _sizeViewWidth = (int)(view.x / _sizeGrid.x) + 1;
            _sizeViewHeight = (int)(view.y / _sizeGrid.y) + 1;
            _countTilesInUpdate = (int)(camera.orthographicSize * camera.orthographicSize) * 10;
        }

        /// <summary>
        /// Play or pause all map coroutines
        /// </summary>
        public void PlayPause()
        {
            if (update == null)
                Play();
            else
                Pause();
        }
        /// <summary>
        /// Play all map coroutine
        /// </summary>
        public void Play()
        {
            if (!_inited)
                Init();
            if (update == null)
                update = StartCoroutine(UpdateTerrain());
            if (coroutineCollider == null)
                coroutineCollider = StartCoroutine(UpdateCollider());
        }
        /// <summary>
        /// Pause all map coroutine
        /// </summary>
        public void Pause()
        {
            PauseTerrain();
            PauseCollider();
        }
        /// <summary>
        /// Play or pause terrain coroutine
        /// </summary>
        public void PlayPauseTerrain()
        {
            if (update == null)
                PlayTerrain();
            else
                PauseTerrain();
        }
        /// <summary>
        /// Play terrain coroutine
        /// </summary>
        public void PlayTerrain()
        {
            if (!_inited)
                Init();
            if (update == null)
                update = StartCoroutine(UpdateTerrain());
        }
        /// <summary>
        /// Pause terrain coroutine
        /// </summary>
        public void PauseTerrain()
        {
            if (update != null)
            {
                StopCoroutine(update);
                update = null;
            }
        }
        /// <summary>
        /// Play or pause collider coroutine
        /// </summary>
        public void PlayPauseCollider()
        {
            if (coroutineCollider == null)
                PlayCollider();
            else
                PauseCollider();
        }
        /// <summary>
        /// Play collider coroutine
        /// </summary>
        public void PlayCollider()
        {
            if (!_inited)
                Init();
            if (coroutineCollider == null)
                coroutineCollider = StartCoroutine(UpdateCollider());
        }
        /// <summary>
        /// Pause collider coroutine
        /// </summary>
        public void PauseCollider()
        {
            if (coroutineCollider != null)
            {
                StopCoroutine(coroutineCollider);
                coroutineCollider = null;
            }
        }
        #endregion

        #region Body Private
        private void Awake()
        {
            if (_initOnAwake)
                Init();
        }
        // Map generation coroutine
        private IEnumerator UpdateTerrain()
        {
            while (true)
            {
                yield return null;

                //get current grid position
                Vector3Int tilePos = Pos2Grid(_target.localPosition);

                //Check if target is movement
                if (tilePos - _current != Vector3Int.zero)
                {
                    //Calculate and normalization direction of movement target
                    int x = Mathf.Clamp(tilePos.x - _current.x, -1, 1);
                    int y = Mathf.Clamp(tilePos.y - _current.y, -1, 1);
                    _movement = new Vector2Int(x, y);

                    //Reset environment generation
                    _counter = 0; _current = tilePos;
                    _lengthOfEnvironment = (_offset.x * 2 + 1) * (_offset.y * 2 + 1);

                    //On map generation caller
                    _environmentInProcess = true;
                }

                //Main environment generation process
                if (_environmentInProcess)
                {
                    //How many tiles will be calculate in fixedframe
                    int localLength = _countTilesInUpdate;
                    if (_lengthOfEnvironment - _counter < _countTilesInUpdate)
                        localLength = _lengthOfEnvironment - _counter;

                    //Choose method in which type of queue the tiles will be generated
                    GetPosEnvironment getPos = PosEnvironment.Convert[_movement];

                    //Generate tiles
                    for (int i = 0; i < localLength; i++)
                    {
                        Generate(getPos.Invoke(_counter, _current, _offset));
                        _counter++;
                    }
                }

                if (_counter == _lengthOfEnvironment)
                {
                    //Reset environment generation
                    _counter = 0; _current = tilePos;
                    _lengthOfEnvironment = (_offset.x * 2 + 1) * (_offset.y * 2 + 1);

                    //Clear cache (this method is optional... I don't know)
                    //Caching.ClearCache();

                    //Off map generation caller
                    _environmentInProcess = false;
                }
            }
        }

        // Generate tile with map data
        private void Generate(Vector3Int pos)
        {
            Profiler.BeginSample("1");
            BlockID index = GetBlockID(pos.x, pos.y);
            Profiler.EndSample();
            Block block = _blocks.Convert(index);
            TileBase tile = block.Tile;

            Profiler.BeginSample("2");
            if (block.Wall)
            {
                if (!_walls.HasTile(pos))
                    _walls.SetTile(pos, tile);
            }
            else
            {
                if (!_background.HasTile(pos))
                    _background.SetTile(pos, tile);
            }
            Profiler.EndSample();
        }
        // Get current block from map data
        private BlockID GetBlockID(int x, int y)
        {
            // Out of Borders
            if (Mathf.Abs(x) > _halfSizeMapWidth || Mathf.Abs(y) > _halfSizeMapHeight)
            { return BlockID.Wall; }

            // Objects Map
            if (_generateObjects)
            {
                for (int i = 0; i < _lengthObjects; i++)
                {
                    if (_objects[i].InColliderDetection(new Vector2Int(x, y), out BlockID blockID))
                        return blockID;
                }
            }

            // Natural Map
            if (_generateTerrain)
            {
                float perlinMin = _noise.GetPerlin(x * 5f, y * 5f);
                float cubicMax = _noise.GetCubic(x, y);
                if (perlinMin < -0.35f || cubicMax < -0.1f)
                    return BlockID.Wall;
                return BlockID.Background;
            }

            return BlockID.Background;
        }
        // Map collider coroutine
        private IEnumerator UpdateCollider()
        {
            while (_generateCollider)
            {
                yield return new WaitForSeconds(_timeColliderUpdate);

                //Reset
                Vector3Int tilePos = Pos2Grid(_target.localPosition);
                Vector3Int offset = new Vector3Int(_sizeColliderWidth, _sizeColliderHeight, 0);
                Vector3Int min = tilePos - offset; Vector3Int max = tilePos + offset;

                //Generate collider environment
                for (int x = min.x; x < max.x + 1; x++)
                {
                    for (int y = min.y; y < max.y + 1; y++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        if (_walls.HasTile(pos))
                        {
                            //Set pool object
                            GameObject obj = _colliderPool.Dequeue();
                            obj.transform.localPosition = Grid2Pos(pos);
                            if (!obj.activeSelf) { obj.SetActive(true); }
                            _colliderPool.Enqueue(obj);
                        }
                    }
                }
            }
        }
        #endregion

        #region Static
        /// <summary>
        /// Convert real position to current grid tile position
        /// </summary>
        public static Vector3Int Pos2Grid(Vector3 pos)
        { return new Vector3Int((int)(pos.x / _sizeGrid.x), (int)(pos.y / _sizeGrid.y), 0); }
        /// <summary>
        /// Convert current grid tile position to real position
        /// </summary>
        public static Vector3 Grid2Pos(Vector3Int pos)
        { return new Vector3(pos.x * _sizeGrid.x + 0.25f, pos.y * _sizeGrid.y + 0.25f, 0); }

        /// <summary>
        /// This class is only needed to get type of queue the tiles will be generated
        /// </summary>
        private static class PosEnvironment
        {
            private static Dictionary<Vector2Int, GetPosEnvironment> _convert = new Dictionary<Vector2Int, GetPosEnvironment>()
            {
                { new Vector2Int(-1, 0), new GetPosEnvironment(GetPosLD) },
                { new Vector2Int(0, -1), new GetPosEnvironment(GetPosDR) },
                { new Vector2Int(1, 0), new GetPosEnvironment(GetPosRU) },
                { new Vector2Int(0, 1), new GetPosEnvironment(GetPosUL) },
                { new Vector2Int(-1, -1), new GetPosEnvironment(GetPosLD) },
                { new Vector2Int(1, -1), new GetPosEnvironment(GetPosDR) },
                { new Vector2Int(1, 1), new GetPosEnvironment(GetPosRU) },
                { new Vector2Int(-1, 1), new GetPosEnvironment(GetPosUL) }
            };
            public static Dictionary<Vector2Int, GetPosEnvironment> Convert => _convert;

            private static Vector3Int GetPosLD(int count, Vector3Int current, Vector2Int offset)
            {
                int sizeY = offset.y * 2 + 1;
                int x = Mathf.FloorToInt(count / sizeY); int y = count - x * sizeY;
                return current + new Vector3Int(x - offset.x, y - offset.y, 0);
            }
            private static Vector3Int GetPosDR(int count, Vector3Int current, Vector2Int offset)
            {
                int sizeX = offset.x * 2 + 1;
                int y = Mathf.FloorToInt(count / sizeX); int x = count - y * sizeX;
                return current + new Vector3Int(offset.x - x, y - offset.y, 0);
            }
            private static Vector3Int GetPosUL(int count, Vector3Int current, Vector2Int offset)
            {
                int sizeX = offset.x * 2 + 1;
                int y = Mathf.FloorToInt(count / sizeX); int x = count - y * sizeX;
                return current + new Vector3Int(x - offset.x, offset.y - y, 0);
            }
            private static Vector3Int GetPosRU(int count, Vector3Int current, Vector2Int offset)
            {
                int sizeY = offset.y * 2 + 1;
                int x = Mathf.FloorToInt(count / sizeY); int y = count - x * sizeY;
                return current + new Vector3Int(offset.x - x, offset.y - y, 0);
            }
        }
        public delegate Vector3Int GetPosEnvironment(int count, Vector3Int current, Vector2Int offset);
        #endregion
    }
}