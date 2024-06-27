using MapSystem2D.Spawn;
using UnityEngine;

namespace MapSystem2D
{
    [CreateAssetMenu(fileName = "ObjectData", menuName = "Data/Object Data", order = 0)]
    public class ObjectMapData : ScriptableObject
    {
        [SerializeField] private Texture2D data;
        [SerializeField] private PatternPosition patternPosition;
        [SerializeField] private float distTrig2Calc = 100f;
        private BlockID[,] _data;

        public void Init()
        {
            _data = new BlockID[data.width, data.height];
            for (int i = 0; i < data.width; i++)
                for (int j = 0; j < data.height; j++)
                    _data[i, j] = Map.Blocks.Convert(data.GetPixel(i, j));
        }

        public bool InColliderDetection(Vector2Int pos, out BlockID idBlock)
        {
            idBlock = BlockID.Background;
            Vector2Int position = patternPosition.GetPosition(pos);
            if (Vector2.Distance(position, pos) > distTrig2Calc)
                return false;

            Vector2Int size = new Vector2Int(data.width, data.height);
            Vector2Int start = position - size / 2;
            Vector2Int end = start + size;

            bool x = start.x <= pos.x && pos.x < end.x;
            bool y = start.y <= pos.y && pos.y < end.y;
            if (x && y)
            {
                idBlock = _data[pos.x - start.x, pos.y - start.y];
                return true;
            }
            return false;
        }
    }
}