using UnityEngine;

namespace MapSystem2D.Spawn
{
    /// <summary>
    /// Base class for get object position
    /// </summary>
    public class PatternPosition : ScriptableObject
    {
        public virtual Vector2Int GetPosition(Vector2Int pos)
        {
            return Vector2Int.zero;
        }
    }
}