using System.Collections.Generic;
using NN.Core;
using NN.Training.Data;

namespace NN.Training.Propagation
{
    public class BackPropagation
    {
        private NeuralNetwork network;

        public NeuralNetwork Network => network;

        public BackPropagation(NeuralNetwork network)
        {
            this.network = network;
        }

        public void Backward(NetworkSnapshot ns, float error)
        {
            
        }
    }
}
