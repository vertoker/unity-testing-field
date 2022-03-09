using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInitializer : MonoBehaviour
{
    private void OnEnable()
    {
        ChunkDataTransfer.GetContext();
    }
    private void OnDisable()
    {
        
    }
}
