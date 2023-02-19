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
            for (int i = 0; i < length; i++)
                layers[i].layerWeights = nn.Weights[i];
        }

        public NeuralNetwork Load(bool complementWeights = true)
        {
            if (complementWeights)
                ComplementWeights();
            
            var length = layers.Length;
            
            var weights = new float[length][];
            for (int i = 0; i < length; i++)
                weights[i] = layers[i].layerWeights;
            
            return new NeuralNetwork(topology, weights);
        }

        public void ComplementWeights()
        {
            var lengthLayersTarget = topology.Length - 1;
            var lengthLayers = layers.Length;
            if (lengthLayers < lengthLayersTarget)
                Array.Resize(ref layers, lengthLayersTarget);
            
            for (int i = 0; i < lengthLayersTarget; i++)
            {
                var count = (topology[i] + 1) * topology[i + 1];
                if (layers[i].layerWeights.Length < count)
                    Array.Resize(ref layers[i].layerWeights, count);
            }
        }
    }

    [Serializable]
    public class Layer
    {
        public float[] layerWeights;
    }
}