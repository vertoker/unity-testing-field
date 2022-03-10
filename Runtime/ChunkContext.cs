using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChunkContext
{
    private readonly int _sizeX, _sizeY;
    private readonly int _posX, _posY;

    public int SizeX => _sizeX;
    public int SizeY => _sizeY;
    public int PosX => _posX;
    public int PosY => _posY;

    public Vector3 GetPosition()
    {
        return new Vector3(_sizeX * _posX, _sizeY * _posY, 0);
    }
    public Vector2Int GetMinPosition()
    {
        return new Vector2Int(-_sizeX / 2, -_sizeY / 2);
    }
    public Vector2Int GetMaxPosition()
    {
        return new Vector2Int(_sizeX / 2, _sizeY / 2);
    }

    public ChunkContext(int sizeX, int sizeY, int posX, int posY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _posX = posX;
        _posY = posY;
    }
}
