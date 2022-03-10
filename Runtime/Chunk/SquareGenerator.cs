using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Game.WorldGeneration
{
    [CreateAssetMenu(fileName = "New Square Generator", menuName = "Map/Square")]
    public class SquareGenerator : Generator
    {
        [SerializeField] private TileBase _main;
        [SerializeField] private int _sizeHoles = 4;

        public override void Generate(ChunkContext context, Tilemap[] tilemaps)
        {
            Tilemap tilemap = tilemaps[0];
            Vector3Int[] positions = StaticGenerations.HollowRectangle(context.GetMinPosition(), context.GetMaxPosition());
            SetTiles(_main, positions, tilemap);
            Vector3Int[] holes = StaticGenerations.HollowRectangleCentralHoles(context.GetMinPosition(), context.GetMaxPosition(), _sizeHoles);
            SetTiles(null, holes, tilemap);
        }
    }
}