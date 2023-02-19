using NN.Core;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.Serialization;

namespace NN.Saver
{
    [CreateAssetMenu(menuName = "NN/Neural Network Weights", fileName = "NeuralNetworkWeights")]
    public class NeuralNetworkSaverUnity : ScriptableObject
    {
        public int[] editorTopology = { 2, 1 };
        
        public int[] topology = { 2, 1 };
        public float[] weights = { 0f, 0f, 0f };

        public void Save(NeuralNetwork nn)
        {
            topology = nn.Topology;
            var flattened = 
            weights = nn.Weights.SelectMany(inner => inner).ToArray();;
        }

        public NeuralNetwork Load(bool complementWeights = true)
        {
            if (complementWeights)
                ComplementWeights();

            return new NeuralNetwork(topology, weights);
        }

        public void ComplementWeights()
        {
            var length = 0;
            var lengthTopology = topology.Length - 1;
            for (int i = 0; i < lengthTopology; i++)
                length += (topology[i] + 1) * topology[i + 1];
            
            if (weights.Length < length)
                Array.Resize(ref weights, length);
        }

        public void FillEmpty()
        {
            var length = 0;
            var lengthTopology = editorTopology.Length - 1;
            for (int i = 0; i < lengthTopology; i++)
                length += (editorTopology[i] + 1) * editorTopology[i + 1];
            
            topology = editorTopology;
            weights = new float[length];
        }
    }
}