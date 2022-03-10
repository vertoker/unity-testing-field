using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldGeneration
{
    public static class StaticGenerations
    {
        public static Vector3Int[] FillRectangle(Vector2Int min, Vector2Int max)
        {
            int size = (max.x - min.x) * (max.y - min.y);
            Vector3Int[] rect = new Vector3Int[size];

            int counter = 0;
            for (int x = min.x; x <= max.x; x++)
            {
                for (int y = min.y; y < max.y; y++)
                {
                    rect[x * counter + y] = new Vector3Int(x, y, 0);
                }
                counter++;
            }

            return rect;
        }

        public static Vector3Int[] HollowRectangle(Vector2Int min, Vector2Int max)
        {
            int sizeX = max.x - min.x, sizeY = max.y - min.y;
            int size = sizeX + sizeX + sizeY + sizeY;
            if (size == 0)
                size = 1;
            Vector3Int[] rect = new Vector3Int[size];

            //Up and down
            int counter = 0;
            for (int x = min.x; x <= max.x; x++)
            {
                rect[counter] = new Vector3Int(x, max.y, 0);
                counter++;
                rect[counter] = new Vector3Int(x, min.y, 0);
                counter++;
            }

            //Left and right
            for (int y = min.y + 1; y < max.y; y++)
            {
                rect[counter] = new Vector3Int(min.x, y, 0);
                counter++;
                rect[counter] = new Vector3Int(max.x, y, 0);
                counter++;
            }

            return rect;
        }

        public static Vector3Int[] HollowRectangleCentralHoles(Vector2Int min, Vector2Int max, int sizeHoles)
        {
            Vector3Int[] rect = new Vector3Int[sizeHoles * 4];
            int halfSizeHoles = sizeHoles / 2;

            //Up and down
            int counter = 0;
            for (int x = 0; x < sizeHoles; x++)
            {
                rect[counter] = new Vector3Int(x - halfSizeHoles, max.y, 0);
                counter++;
                rect[counter] = new Vector3Int(x - halfSizeHoles, min.y, 0);
                counter++;
            }

            //Left and right
            for (int y = 0; y < sizeHoles; y++)
            {
                rect[counter] = new Vector3Int(min.x, y - halfSizeHoles, 0);
                counter++;
                rect[counter] = new Vector3Int(max.x, y - halfSizeHoles, 0);
                counter++;
            }

            return rect;
        }
    }
}