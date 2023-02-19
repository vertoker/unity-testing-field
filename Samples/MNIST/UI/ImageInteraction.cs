using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace NN.Samples.MNIST.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class ImageInteraction : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public Action OnEnterWhileDown, OnClick;
        private RectTransform self;
        private Image img;

        public void Init()
        {
            self = GetComponent<RectTransform>();
            img = GetComponent<Image>();
        }

        public Vector3 RectPosition
        {
            get => self.anchoredPosition;
            set => self.anchoredPosition = value;
        }
        public void SetColor(Color color)
        {
            img.color = color;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0))
                OnEnterWhileDown?.Invoke();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}
