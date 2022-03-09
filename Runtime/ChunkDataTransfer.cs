using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDataTransfer : MonoBehaviour
{
    private static Queue<ChunkContext> _chunkTasks = new Queue<ChunkContext>();

    public static void SetContext(ChunkContext context)
    {
        _chunkTasks.Enqueue(context);
    }

    public static ChunkContext GetContext()
    {
        return _chunkTasks.Dequeue();
    }
}
