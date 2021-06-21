using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Game.Pool;

public class ScreenSpawner : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    [SerializeField] private PoolSpawner _pool;
    [SerializeField] private int _lifetime;

    public void OnDrag(PointerEventData eventData)
    {
        OnPointerClick(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Transform transform = _pool.Dequeue(_lifetime).transform;
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }

}
