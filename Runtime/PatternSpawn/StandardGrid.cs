using UnityEngine;

namespace Game.Map2D
{
    /// <summary>
    /// Get closest grid position
    /// </summary>
    [CreateAssetMenu(fileName = "Grid", menuName = "Pattern/Grid")]
    public class StandardGrid : PatternPosition
    {
        [SerializeField] private int sizeX = 1000;
        [SerializeField] private int sizeY = 1000;

        public override Vector2Int GetPosition(Vector2Int pos)
        {
            int x = Mathf.RoundToInt((float)pos.x / sizeX);
            int y = Mathf.RoundToInt((float)pos.y / sizeY);
            return new Vector2Int(x * sizeX, y * sizeY);
        }
    }
}