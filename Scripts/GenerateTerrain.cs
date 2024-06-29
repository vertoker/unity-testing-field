using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(30, 30);
    public Vector2 scale = new Vector2(1, 1);
    public Vector2 offset = new Vector2(0, 0);
    public Vector2 speed = new Vector2(0.05f, 0.05f);
    private List<Vector3> positions = new List<Vector3>();
    public InstanceOutput instanceOutput;
    private Transform tr;

    void Start()
    {
        tr = transform;
        instanceOutput.StartRender(size.x * size.y);
    }

    void Update()
    {
        Vector3 pos = tr.localPosition;
        positions = new List<Vector3>();
        offset += speed;

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                int posX = (int)pos.x + x;
                int posZ = (int)pos.z + z;
                float xCoord = (float)posX / size.x * scale.x + offset.x;
                float zCoord = (float)posZ / size.y * scale.y + offset.y;

                int y = (int)(Mathf.PerlinNoise(xCoord, zCoord) * 10f);
                positions.Add(new Vector3(posX, y, posZ));
            }
        }
        instanceOutput.SetPositions(positions);
    }
}