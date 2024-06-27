using UnityEngine;

namespace MapSystem2D.Spawn
{
    /// <summary>
    /// Get closest grid position with random offset
    /// </summary>
    [CreateAssetMenu(fileName = "RandomGrid", menuName = "Pattern/Random Grid")]
    public class RandomRotateGrid : PatternPosition
    {
        [SerializeField] private int sizeX = 1000;
        [SerializeField] private int sizeY = 1000;
        [SerializeField] private float dist = 700f;

        public override Vector2Int GetPosition(Vector2Int pos)
        {
            Vector2Int curPos = GetGridPosition(pos);
            float angle = Map.Noise.GetCubic(curPos.x, curPos.y) * 180f * Mathf.Deg2Rad;
            float power = (Map.Noise.GetCellular(curPos.x, curPos.y) + 1f) / 2f * dist;
            Vector2 rotate = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * power;

            int x = Mathf.RoundToInt((float)pos.x / sizeX);
            int y = Mathf.RoundToInt((float)pos.y / sizeY);
            return new Vector2Int(x * sizeX + (int)rotate.x, y * sizeY + (int)rotate.y);
        }
        private Vector2Int GetGridPosition(Vector2Int pos)
        {
            int x = Mathf.RoundToInt((float)pos.x / sizeX);
            int y = Mathf.RoundToInt((float)pos.y / sizeY);
            return new Vector2Int(x * sizeX, y * sizeY);
        }
    }
}