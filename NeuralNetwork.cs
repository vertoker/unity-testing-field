using System;
using System.Linq;

namespace Bobby.NN
{
    [Serializable]
    public class NeuralNetwork
    {
        private int[] topology;
        private float[][,] weights;
        private Random random;

        public int[] Topology => topology;
        public float[][,] Weights => weights;
        public Random Random => random;
        
        public int this[int index]
        {
            get => topology[index];
            set => topology[index] = value;
        }
        public float this[int index, int row, int column]
        {
            get => weights[index][row, column];
            set => weights[index][row, column] = value;
        }
        
        public int NeuronsCount => topology.Sum();
        public int InputNeuronsCount => topology[0];
        public int OutputNeuronsCount => topology[^1];
        public int HiddenNeuronsCount => topology.Sum() - topology[0] - topology[^1];
        
        public int WeightsCount
        {
            get
            {
                int sum = 0, length = topology.Length - 1;
                for (int counter = 0; counter < length; counter++)
                    sum += topology[counter] * topology[counter + 1];
                return sum;
            }
        }
        public int InputWeightsCount => topology[0] * topology[1];
        public int OutputWeightsCount => topology[^1] * topology[^2];
        public int HiddenWeightsCount
        {
            get
            {
                int sum = 0, length = topology.Length - 2;
                for (int counter = 1; counter < length; counter++)
                    sum += topology[counter] * topology[counter + 1];
                return sum;
            }
        }

        public int GetLayerWeightsCount(int layer) => topology[layer] * topology[layer + 1];
        public int GetLayerNeuronCount(int layer) => topology[layer] + topology[layer + 1];

        public NeuralNetwork(int[] topology)
        {
            random = new Random();
            this.topology = topology;
            int length = topology.Length - 1;

            weights = new float[length][,];
            for (int i = 0; i < length; i++)
                weights[i] = new float[topology[i], topology[i] + 1];
        }
        public NeuralNetwork(float[][,] weights)
        {
            random = new Random();
            this.weights = weights;
            int length = weights.Length;
            topology = new int[length + 1];

            for (int i = 0; i < length; i++)
                topology[i] = weights[i].GetLength(0);
            topology[length] = weights[length - 1].GetLength(1);
        }
        public NeuralNetwork(int[] topology, float[][,] weights)
        {
            random = new Random();
            this.topology = topology;
            this.weights = weights;
        }
        public NeuralNetwork(int[] topology, float[][,] weights, Random random)
        {
            this.random = random;
            this.topology = topology;
            this.weights = weights;
        }

        /// <summary>
        /// Main method of NN
        /// </summary>
        public float[] Forward(float[] inputs, ActivationType activationType = ActivationType.Sigmoid)
        {
            return Forward(inputs, Activations.Get(activationType));
        }
        /// <summary>
        /// Main method of NN
        /// </summary>
        public float[] Forward(float[] inputs, ActivationCalculate activator)
        {
            var length = weights.Length;
            var inputCount = topology[0];
            
            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                var outputs = new float[outputCount];
                
                for (int i = 0; i < inputCount; i++)
                {
                    for (int j = 0; j < outputCount; j++)
                    {
                        outputs[j] += inputs[i] * weights[outputCounter][i, j];
                    }
                }
                
                inputCount = outputCount;
                inputs = Activations.NormalizeNeurons(outputs, activator);
            }
            
            return inputs;
        }

        /// <summary>
        /// Main method of NN with values of every neuron
        /// Important: progress include inputs
        /// </summary>
        public float[][] ForwardWithProgress(float[] inputs, ActivationType activationType = ActivationType.Sigmoid)
        {
            return ForwardWithProgress(inputs, Activations.Get(activationType));
            
        }
        /// <summary>
        /// Main method of NN with values of every neuron
        /// Important: progress include inputs
        /// </summary>
        public float[][] ForwardWithProgress(float[] inputs, ActivationCalculate activator)
        {
            var length = weights.Length;
            var inputCount = topology[0];
            
            float[][] result = new float[length + 1][];
            result[0] = inputs;
            
            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                var outputs = new float[outputCount];
                
                for (int i = 0; i < inputCount; i++)
                {
                    for (int j = 0; j < outputCount; j++)
                    {
                        outputs[j] += inputs[i] * weights[outputCounter][i, j];
                    }
                }

                inputCount = outputCount;
                result[outputCounter + 1] = outputs;
                inputs = Activations.NormalizeNeurons(outputs, activator);
            }
            
            return result;
        }

        /// <summary>
        /// This method set random weights in all NN
        /// </summary>
        public void RandomizeWeights(float min, float max)
        {
            var length = weights.Length;
            var range = max - min;
            var inputCount = topology[0];

            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                
                for (int i = 0; i < inputCount; i++)
                {
                    for (int j = 0; j < outputCount; j++)
                    {
                        weights[outputCounter][i, j] = (float)random.NextDouble() * range - min;
                    }
                }

                inputCount = outputCount;
            }
        }

        public WeightModification GetRandomWeightModification()
        {
            var randomLayer = random.Next(0, weights.Length);
            var randomInputWeight = random.Next(0, topology[randomLayer]);
            var randomOutputWeight = random.Next(0, topology[randomLayer]);
            return GetWeightModification(randomLayer, randomInputWeight, randomOutputWeight);
        }
        public WeightModification GetWeightModification(int layer, int inputWeight, int outputWeight)
        {
            return value => this[layer, inputWeight, outputWeight] = value;
        }

        public void ApplyWeightsDelta(float[][,] weightsDelta)
        {
            var length = weights.Length;
            
            for (int i = 0; i < length; i++)
            {
                var inputCount = topology[i];
                var outputCount = topology[i + 1];

                for (int j = 0; j < inputCount; j++)
                {
                    for (int k = 0; k < outputCount; k++)
                    {
                        weights[i][j, k] += weightsDelta[i][j, k];
                    }
                }
            }
        }

        /// <summary>
        /// This method return new class with same parameters
        /// </summary>
        public NeuralNetwork Copy(Random customRandom = null)
        {
            customRandom ??= new Random();
            return new NeuralNetwork(topology, weights, customRandom);
        }
    }
    public delegate float WeightModification(float value);
}
