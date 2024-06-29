using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjData
{
    public Vector3 pos;
    public Vector3 scale;
    public Quaternion rot;

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);
        }
    }

    public ObjData(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
    }
}

public class InstanceOutput : MonoBehaviour
{
    public int instances;
    public Vector3 maxPos;
    public Mesh objMesh;
    public Material objMat;

    List<ObjData> batches = new List<ObjData>();

    public void StartRender(int instances)
    {
        this.instances = instances;
        for (int batchIndexNum = 0; batchIndexNum < instances; batchIndexNum++)
        {
            float x = Random.Range(-maxPos.x, maxPos.x);
            float y = Random.Range(-maxPos.y, maxPos.y);
            float z = Random.Range(-maxPos.z, maxPos.z);
            Vector3 pos = new Vector3(x, y, z);

            batches.Add(new ObjData(pos, new Vector3(1, 1, 1), Quaternion.identity));
        }
    }

    void Update()
    {
        Graphics.DrawMeshInstanced(objMesh, 0, objMat, batches.Select((a) => a.matrix).ToList());
    }

    public void SetPositions(List<Vector3> positions)
    {
        for (int i = 0; i < instances; i++)
        {
            batches[i].pos = positions[i];
        }
    }
}