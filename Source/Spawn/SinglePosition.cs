using UnityEngine;

namespace MapSystem2D.Spawn
{
    /// <summary>
    /// Get position
    /// </summary>
    [CreateAssetMenu(fileName = "Position", menuName = "Pattern/Position")]
    public class SinglePosition : PatternPosition
    {
        [SerializeField] private int posX = 600;
        [SerializeField] private int posY = -600;

        public override Vector2Int GetPosition(Vector2Int pos)
        {
            return new Vector2Int(posX, posY);
        }
    }
}