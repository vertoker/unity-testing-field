using System;

namespace NN.Core
{
    [Serializable]
    public class NetworkSnapshot
    {
        private readonly float[][] neurons;
        
        public float[][] Neurons => neurons;

        public float[] this[int layer]
        {
            get => neurons[layer];
            set => neurons[layer] = value;
        }
        public float this[int layer, int row]
        {
            get => neurons[layer][row];
            set => neurons[layer][row] = value;
        }

        public int LayerCount => neurons.Length;

        public float[] Inputs => neurons[0];
        public float[] Outputs => neurons[^1];

        public NetworkSnapshot(int[] topology)
        {
            var length = topology.Length;
            neurons = new float[length][];
            
            for (int i = 0; i < length; i++)
            {
                neurons[i] = new float[topology[i]];
            }
        }
    }
}