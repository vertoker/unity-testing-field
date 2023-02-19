using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Xml;

namespace NN.Samples.MNIST.UI
{
    public class ImageDrawer : MonoBehaviour
    {
        [SerializeField] private int width = 20;//x
        [SerializeField] private int height = 20;//y
        [SerializeField] private Transform pixelsParent;
        [Space]
        [SerializeField] private UnityEvent<int, int, Color> pixelChanged;
        
        private Color currentColor = Color.white;
        private ImageInteraction[,] pixels;

        private void Awake()
        {
            pixels = new ImageInteraction[width, height];

            var counter = 0;
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixels[x, y] = pixelsParent.GetChild(counter++).GetComponent<ImageInteraction>();
                    pixels[x, y].Init();
                }
            }
        }
        private void OnEnable()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int newX = x, newY = y;
                    pixels[x, y].OnEnterWhileDown += () => DrawPixel(newX, newY);
                }
            }
        }
        private void OnDisable()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pixels[x, y].OnEnterWhileDown = null;
                }
            }
        }
        private void Start()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    DrawPixel(x, y);
                }
            }
        }

        public void Clear()
        {
            var tempColor = currentColor;
            currentColor = Color.white;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    DrawPixel(x, y);
                }
            }
            currentColor = tempColor;
        }
        public void ColorChange(Color color)
        {
            currentColor = color;
        }
        private void DrawPixel(int x, int y)
        {
            pixels[x, y].SetColor(currentColor);
            pixelChanged.Invoke(x, y, currentColor);
        }
    }
}
