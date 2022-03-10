using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldGeneration
{
    public static class PosEnvironment
    {
        private static Vector2Int _vectorL = new Vector2Int(-1, 0);
        private static Vector2Int _vectorR = new Vector2Int(1, 0);
        private static Vector2Int _vectorD = new Vector2Int(0, -1);
        private static Vector2Int _vectorU = new Vector2Int(0, 1);

        private static Vector2Int[] _deltaR = new Vector2Int[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1)
        };
        private static Vector2Int[] _deltaL = new Vector2Int[]
        {
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1)
        };
        private static Vector2Int[] _deltaU = new Vector2Int[]
        {
            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1)
        };
        private static Vector2Int[] _deltaD = new Vector2Int[]
        {
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1)
        };

        private static Dictionary<Vector2Int, Vector2Int[]> _getDelete = new Dictionary<Vector2Int, Vector2Int[]>()
        {
            { _vectorL, _deltaR },//L
            { _vectorR, _deltaL },//R
            { _vectorU, _deltaD },//U
            { _vectorD, _deltaU }//D
        };
        private static Dictionary<Vector2Int, Vector2Int[]> _getSpawn = new Dictionary<Vector2Int, Vector2Int[]>()
        {
            { _vectorL, _deltaL },//L
            { _vectorR, _deltaR },//R
            { _vectorU, _deltaU },//U
            { _vectorD, _deltaD }//D
        };

        public static Vector2Int[] GetDeleteOffset(Vector2Int vector)
        {
            return _getDelete[vector];
        }
        public static Vector2Int[] GetSpawnOffset(Vector2Int vector)
        {
            return _getSpawn[vector];
        }
    }
}