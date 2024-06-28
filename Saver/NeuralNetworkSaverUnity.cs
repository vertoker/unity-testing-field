using System;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace NeuralNetworkPipeline.Saver
{
    [CreateAssetMenu(menuName = "NN/Neural Network Weights", fileName = "NeuralNetworkWeights")]
    public class NeuralNetworkSaverUnity : ScriptableObject
    {
#if UNITY_EDITOR
        public int[] editorTopology = { 2, 1 };
#endif
        public int[] topology = { 2, 1 };
        public float[] weights = { 0f, 0f, 0f };

        public void Save(NeuralNetwork nn)
        {
            topology = nn.Topology;
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

#if UNITY_EDITOR
        public void FillEmpty()
        {
            var length = 0;
            var lengthTopology = editorTopology.Length - 1;
            for (int i = 0; i < lengthTopology; i++)
                length += (editorTopology[i] + 1) * editorTopology[i + 1];
            
            topology = editorTopology;
            weights = new float[length];
        }
#endif
        
#if UNITY_EDITOR
        public void RandomizeWeights(float min, float max)
        {
            var length = 0;
            var lengthTopology = editorTopology.Length - 1;
            for (int i = 0; i < lengthTopology; i++)
                length += (editorTopology[i] + 1) * editorTopology[i + 1];
            
            topology = editorTopology;
            weights = new float[length];
            for (int i = 0; i < length; i++)
                weights[i] = UnityRandom.Range(min, max);
        }
#endif
    }
}