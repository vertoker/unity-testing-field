using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldGeneration
{
    public static class PosEnvironment
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
        //Исправить
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
}