using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ChunkDataTransfer : MonoBehaviour
{
    private static Queue<ChunkContext> _chunkTasks = new Queue<ChunkContext>();

    private static UnityEvent<int, ChunkContext> _sceneGetContext = new UnityEvent<int, ChunkContext>();

    public static event UnityAction<int, ChunkContext> SceneGetContext
    {
        add => _sceneGetContext.AddListener(value);
        remove => _sceneGetContext.RemoveListener(value);
    }

    public static void SetContext(ChunkContext context)
    {
        _chunkTasks.Enqueue(context);
    }

    public static ChunkContext GetContext(int hashCode)
    {
        ChunkContext context = _chunkTasks.Dequeue();
        _sceneGetContext.Invoke(hashCode, context);
        return context;
    }
}
