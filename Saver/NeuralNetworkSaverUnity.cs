using NN.Core;
using UnityEngine;
using System;

namespace NN.Saver
{
    [CreateAssetMenu(menuName = "NN/Neural Network Weights", fileName = "NeuralNetworkWeights")]
    public class NeuralNetworkSaverUnity : ScriptableObject
    {
        [SerializeField] private int[] topology;
        [SerializeField] private Layer[] layers;

        public void Save(NeuralNetwork nn)
        {
            topology = nn.Topology;
            var length = nn.Weights.Length;
            layers = new Layer[length]; 
        }
    }

    [Serializable]
    public class Layer
    {
        [SerializeField] private float[] layerWeights;

        public void Set(int inputLayer, int outputLayer, float[,] weights)
        {
            layerWeights = new float[inputLayer * outputLayer];

            var counter = 0;
            for (int x = 0; x < inputLayer; x++)
            {
                for (int y = 0; y < outputLayer; y++)
                {
                    
                }
            }
        }
    }
}