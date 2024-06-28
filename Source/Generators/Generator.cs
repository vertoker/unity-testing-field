using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapChunkSystem2D.Generators
{
    public class Generator : ScriptableObject
    {
        public virtual void Generate(ChunkContext context, Tilemap[] tilemaps)
        {

        }

        public static void SetTiles(TileBase tile, Vector3Int[] positions, Tilemap tilemap)
        {
            foreach (var position in positions)
            {
                tilemap.SetTile(position, tile);
            }
        }
    }
}
