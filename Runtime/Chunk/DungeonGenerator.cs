using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Game.WorldGeneration
{
    [CreateAssetMenu(fileName = "New Dungeon Generator", menuName = "Map/Dungeon")]
    public class DungeonGenerator : Generator
    {
        [SerializeField] private TileBase _main;
        [SerializeField] private int _divideIterations = 12;
        [SerializeField] private int _minRoomSize = 3;

        public override void Generate(ChunkContext context, Tilemap[] tilemaps)
        {
            Tilemap tilemap = tilemaps[0];
            List<Room> rooms = RoomGenerator(context.GetMinPosition(), context.GetMaxPosition());
            List<Vector3Int> positions = new List<Vector3Int>();
            foreach (var room in rooms)
            {
                positions.AddRange(StaticGenerations.HollowRectangle(room.min, room.max));
            }
            SetTiles(_main, positions.ToArray(), tilemap);
        }

        private List<Room> RoomGenerator(Vector2Int min, Vector2Int max)
        {
            List<Room> rooms = new List<Room>() { new Room() { min = min, max = max } };
            if (_divideIterations > 0)
            {
                bool vertical = Random.value < 0.5f;
                rooms.Add(Divide(rooms[0], vertical));

                for (int i = 0; i < _divideIterations; i++)
                {
                    Room room = SelectBiggestRoom(rooms, out vertical);
                    rooms.Add(Divide(room, vertical));
                }
            }
            return rooms;
        }
        private Room SelectBiggestRoom(List<Room> rooms, out bool vertical)
        {
            int biggest = 0;
            for (int i = 1; i < rooms.Count; i++)
            {
                if (rooms[biggest].IsBigger(rooms[i]))
                {
                    biggest = i;
                }
            }
            vertical = !rooms[biggest].Vertical();
            return rooms[biggest];
        }
        private Room Divide(Room room, bool vertical)
        {
            if (vertical)
            {
                int min = room.min.x + _minRoomSize + 1;
                int max = room.max.x - _minRoomSize - 2;
                if (min > max)
                    return null;
                int divideX = Random.Range(min, max);
                Room newRoom = new Room()
                {
                    min = new Vector2Int(divideX, room.min.y),
                    max = room.max
                };
                room.max.x = divideX;
                return newRoom;
            }
            else
            {
                int min = room.min.y + _minRoomSize + 1;
                int max = room.max.y - _minRoomSize - 2;
                if (min > max)
                    return null;
                int divideY = Random.Range(min, max);
                Room newRoom = new Room()
                {
                    min = new Vector2Int(room.min.x, divideY),
                    max = room.max
                };
                room.max.y = divideY;
                return newRoom;
            }
        }

        [System.Serializable]
        private class Room
        {
            public Vector2Int min, max;

            public override string ToString()
            {
                return string.Format("min: {0}, {1} ||| max: {2}, {3}", min.x, min.y, max.x, max.y);
            }

            public bool IsBigger(Room room)
            {
                return room.GetSquare() > GetSquare();
            }

            public int GetSquare()
            {
                return (max.y - min.y) * (max.x - min.x);
            }

            public bool Vertical()
            {
                return (max.y - min.y) > (max.x - min.x);
            }
        }
    }
}