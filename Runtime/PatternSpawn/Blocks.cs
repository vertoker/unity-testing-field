using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;


namespace Game.Map2D
{
    public enum BlockID
    {
        Background = 0,
        Wall = 1
    }
    [CreateAssetMenu(fileName = "Blocks", menuName = "Data/Blocks", order = 1)]
    public class Blocks : ScriptableObject
    {
        [SerializeField] private Block _background;
        [SerializeField] private Block _walls;

        public Block Convert(BlockID block)
        {
            switch (block)
            {
                case BlockID.Background:
                    return _background;
                case BlockID.Wall:
                    return _walls;
                default:
                    return _background;
            }
        }
        
        public BlockID Convert(Color color)
        {
            if (_walls.Check(color))
                return BlockID.Wall;
            return BlockID.Background;
        }
    }
}