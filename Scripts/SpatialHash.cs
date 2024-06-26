// Credits
// Original author is Leopotam (2023)
// Modified by vertoker (2024)

using System;
using System.Collections.Generic;

namespace SpatialHashing
{
    public class SpatialHash<T>
    {
        public struct Hit {
            public T Item;
            public float DistSqr;
        }
        
        private struct Entry
        {
            public T Item;
            public float X, Y, Z;

            public float GetSqrDist(float xPos, float yPos, float zPos)
            {
                var xDiff = xPos - X;
                var yDiff = yPos - Y;
                var zDiff = zPos - Z;
                return xDiff * xDiff + yDiff * yDiff + zDiff * zDiff;
            }
        }

        private readonly List<Entry>[] _space;
        
        private readonly float _invCellSize;
        private readonly float _minX, _minY, _minZ;
        private readonly int _spaceX, _spaceY, _spaceZ;

        private const int DefaultCellCapacity = 64;
        private readonly List<List<Entry>> _disabledLists;
        private readonly List<List<Entry>> _enabledLists;
        
        public SpatialHash(float cellSize, 
            float minX, float minY, float minZ, 
            float maxX, float maxY, float maxZ)
        {
            if (cellSize <= 0f)
                throw new Exception();
            
            _invCellSize = 1f / cellSize;
            _spaceX = (int)Math.Ceiling((maxX - minX) * _invCellSize);
            _spaceY = (int)Math.Ceiling((maxY - minY) * _invCellSize);
            _spaceZ = (int)Math.Ceiling((maxZ - minZ) * _invCellSize);
            
            if (_spaceX <= 0 || _spaceY <= 0 || _spaceZ <= 0)
                throw new Exception();
            
            (_minX, _minY, _minZ) = (minX, minY, minZ);
            _space = new List<Entry>[_spaceX * _spaceY * _spaceZ];
            _disabledLists = new List<List<Entry>>(_space.Length);
            _enabledLists = new List<List<Entry>>(_space.Length);
        }
        
        public void Add(T item, float xPos, float yPos, float zPos)
        {
            var indexHash = IndexPointHash(xPos, yPos, zPos);
            var list = _space[indexHash];
            
            if (list == null) {
                list = PullFromDisabled();
                _space[indexHash] = list;
                _enabledLists.Add(list);
            }
            
            list.Add(new Entry { Item = item, X = xPos, Y = yPos, Z = zPos });
        }
        public void Clear()
        {
            Array.Clear(_space, 0, _space.Length);
            foreach (var list in _enabledLists)
            {
                list.Clear();
                PushToDisabled(list);
            }
            _enabledLists.Clear();
        }

        #region Search
        public bool Has(float xPos, float yPos, float zPos, float radius, bool selfIgnore)
        {
            var radiusSqr = radius * radius;
            
            var minCellX = WorldToCell(xPos - radius, _minX, _spaceX);
            var minCellY = WorldToCell(yPos - radius, _minY, _spaceY);
            var minCellZ = WorldToCell(zPos - radius, _minZ, _spaceZ);
            var maxCellX = WorldToCell(xPos + radius, _minX, _spaceX);
            var maxCellY = WorldToCell(yPos + radius, _minY, _spaceY);
            var maxCellZ = WorldToCell(zPos + radius, _minZ, _spaceZ);
            
            for (var zCell = minCellZ; zCell <= maxCellZ; zCell++)
            {
                for (var yCell = minCellY; yCell <= maxCellY; yCell++)
                {
                    for (var xCell = minCellX; xCell <= maxCellX; xCell++)
                    {
                        var list = _space[IndexHash(xCell, yCell, zCell)];
                        if (list == null) continue;
                        
                        foreach (var entry in list)
                        {
                            var distSqr = entry.GetSqrDist(xPos, yPos, zPos);
                            if (distSqr > radiusSqr) continue;
                            if (distSqr < 1e-4f && selfIgnore) continue;
                                
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }
        public (Hit hit, bool ok) GetOne(float xPos, float yPos, float zPos,
            float radius, bool selfIgnore)
        {
            var hit = new Hit
            {
                Item = default,
                DistSqr = radius * radius
            };
            var found = false;
            
            var minCellX = WorldToCell(xPos - radius, _minX, _spaceX);
            var minCellY = WorldToCell(yPos - radius, _minY, _spaceY);
            var minCellZ = WorldToCell(zPos - radius, _minZ, _spaceZ);
            var maxCellX = WorldToCell(xPos + radius, _minX, _spaceX);
            var maxCellY = WorldToCell(yPos + radius, _minY, _spaceY);
            var maxCellZ = WorldToCell(zPos + radius, _minZ, _spaceZ);
            
            for (var zCell = minCellZ; zCell <= maxCellZ; zCell++)
            {
                for (var yCell = minCellY; yCell <= maxCellY; yCell++)
                {
                    for (var xCell = minCellX; xCell <= maxCellX; xCell++)
                    {
                        var list = _space[IndexHash(xCell, yCell, zCell)];
                        if (list == null) continue;
                        
                        foreach (var entry in list)
                        {
                            var distSqr = entry.GetSqrDist(xPos, yPos, zPos);
                            if (distSqr > hit.DistSqr) continue;
                            if (distSqr < 1e-4f && selfIgnore) continue;
                            
                            found = true;
                            hit.DistSqr = distSqr;
                            hit.Item = entry.Item;
                        }
                    }
                }
            }
            
            return (hit, found);
        }
        public List<Hit> Get(float xPos, float yPos, float zPos, float radius, 
            bool selfIgnore, List<Hit> result = default)
        {
            if (result == null)
                result = new List<Hit>(DefaultCellCapacity);
            else result.Clear();
            
            var radiusSqr = radius * radius;
            
            var minCellX = WorldToCell(xPos - radius, _minX, _spaceX);
            var minCellY = WorldToCell(yPos - radius, _minY, _spaceY);
            var minCellZ = WorldToCell(zPos - radius, _minZ, _spaceZ);
            var maxCellX = WorldToCell(xPos + radius, _minX, _spaceX);
            var maxCellY = WorldToCell(yPos + radius, _minY, _spaceY);
            var maxCellZ = WorldToCell(zPos + radius, _minZ, _spaceZ);
            
            for (var zCell = minCellZ; zCell <= maxCellZ; zCell++)
            {
                for (var yCell = minCellY; yCell <= maxCellY; yCell++)
                {
                    for (var xCell = minCellX; xCell <= maxCellX; xCell++)
                    {
                        var list = _space[IndexHash(xCell, yCell, zCell)];
                        if (list == null) continue;
                        
                        foreach (var entry in list)
                        {
                            var distSqr = entry.GetSqrDist(xPos, yPos, zPos);
                            if (distSqr > radiusSqr) continue;
                            if (distSqr < 1e-4f && selfIgnore) continue;
                            
                            result.Add(new Hit { Item = entry.Item, DistSqr = distSqr });
                        }
                    }
                }
            }
            
            // Use sort yourself
            //if (result.Count > 1) result.Sort(OnSort);
            
            return result;
        }
        #endregion

        #region Utils
        /// <summary> Use this for sorting hit results </summary>
        public static int OnSort(Hit x, Hit y) => x.DistSqr < y.DistSqr ? -1 : 1;
        
        /// <summary> Custom hash function, use as indexing of space, for world point </summary>
        public int IndexPointHash(float xPos, float yPos, float zPos) {
            var xCell = WorldToCell(xPos, _minX, _spaceX);
            var yCell = WorldToCell(yPos, _minY, _spaceY);
            var zCell = WorldToCell(zPos, _minZ, _spaceZ);
            return IndexHash(xCell, yCell, zCell);
        }
        
        /// <summary> Custom hash function, use as indexing of space, for cell point </summary>
        public int IndexHash(int xCell, int yCell, int zCell)
            => zCell * _spaceX * _spaceY + yCell * _spaceX + xCell;
        
        private List<Entry> PullFromDisabled()
        {
            var count = _disabledLists.Count;
            if (count <= 0) return new List<Entry>(DefaultCellCapacity);
            
            count--;
            var list = _disabledLists[count];
            _disabledLists.RemoveAt(count);
            return list;
        }
        private void PushToDisabled(List<Entry> list) => _disabledLists.Add(list);
        
        private int WorldToCell(float worldAxis, float worldMin, int spaceLength)
        {
            // Here's error for negative number, but next will be clamp to 0
            var result = (int)((worldAxis - worldMin) * _invCellSize);
            
            // Clamp by 0 and length-1
            if (result >= spaceLength)
                result = spaceLength - 1;
            else if (result < 0)
                result = 0;
            
            return result;
        }
        #endregion
    }
}