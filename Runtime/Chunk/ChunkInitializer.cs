using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ChunkInitializer : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    private Tilemap[] _tilemaps;
    private ChunkContext _context;

    private void OnEnable()
    {
        _context = ChunkDataTransfer.GetContext();
        transform.position = _context.GetPosition();
        _tilemaps = new Tilemap[transform.childCount];
        for (int i = 0; i < _tilemaps.Length; i++)
        {
            _tilemaps[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }

        _generator.Initialize(_context, _tilemaps);
    }
    private void OnDisable()
    {
        
    }
}