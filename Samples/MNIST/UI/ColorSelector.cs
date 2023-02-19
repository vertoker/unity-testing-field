using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SearchService;

namespace NN.Samples.MNIST.UI
{
    public class ColorSelector : MonoBehaviour
    {
        [SerializeField] private ImageInteraction selectWhite;
        [SerializeField] private ImageInteraction selectBlack;
        [SerializeField] private ImageInteraction selectionVisualizer;
        [Space]
        [SerializeField] private UnityEvent<Color> colorSelected;

        private void Awake()
        {
            selectWhite.Init();
            selectBlack.Init();
            selectionVisualizer.Init();
        }
        private void OnEnable()
        {
            selectWhite.OnClick += () => Select(Color.white, selectWhite.RectPosition);
            selectBlack.OnClick += () => Select(Color.black, selectBlack.RectPosition);
        }
        private void OnDisable()
        {
            selectWhite.OnClick = null;
            selectBlack.OnClick = null;
        }
        private void Start()
        {
            Select(Color.black, selectBlack.RectPosition);
        }
        private void Select(Color color, Vector3 position)
        {
            selectionVisualizer.RectPosition = position;
            colorSelected.Invoke(color);
        }
    }
}
