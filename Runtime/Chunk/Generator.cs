using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

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
