using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Game.Pool;

public class ScreenSpawner : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PoolSpawner pool;

    public void OnPointerClick(PointerEventData eventData)
    {
        Transform transform = pool.Dequeue(15).transform;
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }
}
