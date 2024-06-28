using System;
using System.Linq;
using SystemRandom = System.Random;
using UnityRandom = UnityEngine.Random;

namespace NN.Core
{
    public delegate void WeightSet(float value);
    public delegate float WeightGet();
    
    /// <summary>
    /// Neural Network Unit
    /// </summary>
    [Serializable]
    public class NeuralNetwork
    {
        private readonly int[] topology;
        private readonly float[][] weights;
        
        /// <summary>
        /// Topology of network, information about count of neurons between every weight layer
        /// Doesn't include biases neurons
        /// </summary>
        public int[] Topology => topology;
        /// <summary>
        /// All network weights, represents by layer between neuron columns
        /// Doesn't contains input biases weights
        /// Contain only output biases weights
        /// </summary>
        public float[][] Weights => weights;
        
        /// <summary>
        /// Get neuron layer count
        /// You can't set, because topology using for other network functions
        /// </summary>
        /// <param name="index">Represents neuron layer index</param>
        public int this[int index] => topology[index];
        /// <summary>
        /// Get/Set any weight in network
        /// It's just get last row of weights in selected layer
        /// </summary>
        /// <param name="layer">Represents layer index</param>
        /// <param name="counter">Represents weight in layer index</param>
        public float this[int layer, int counter]
        {
            get => weights[layer][counter];
            set => weights[layer][counter] = value;
        }
        /// <summary>
        /// Get/Set any weight in network, but with slower 2 dimensional indexes
        /// Use this[layer, counter] instead of this[layer, inputIndex, outputIndex]
        /// Also include biases weights in LAST ROW of every layer
        /// </summary>
        /// <param name="layer">Represents layer index</param>
        /// <param name="inputIndex">Represents input neuron index</param>
        /// <param name="outputIndex">Represents output neuron index</param>
        public float this[int layer, int inputIndex, int outputIndex]
        {
            get => weights[layer][inputIndex * topology[layer] + outputIndex];
            set => weights[layer][inputIndex * topology[layer] + outputIndex] = value;
        }

        #region Fields
        /// <summary>
        /// Count of all neurons: inputs, output, hidden
        /// Doesn't include biases
        /// </summary>
        public int NeuronsCount => topology.Sum();
        /// <summary>
        /// Count of all neurons: inputs, output, hidden
        /// Contains biases too
        /// </summary>
        public int FullNeuronsCount => topology.Sum() + topology.Length - 1;
        /// <summary>
        /// Input neurons count
        /// Doesn't include bias neuron
        /// </summary>
        public int InputNeuronsCount => topology[0];
        /// <summary>
        /// Output neurons count
        /// </summary>
        public int OutputNeuronsCount => topology[topology.Length - 1];
        /// <summary>
        /// Hidden neurons count
        /// </summary>
        public int HiddenNeuronsCount => topology.Sum() - topology[0] - topology[topology.Length - 1];
        /// <summary>
        /// Count of all weights
        /// Include biases output weights
        /// Doesn't include biases input weights
        /// </summary>
        public int WeightsCount
        {
            get
            {
                int sum = 0, length = topology.Length - 1;
                for (int counter = 0; counter < length; counter++)
                    sum += (topology[counter] + 1) * topology[counter + 1];
                return sum;
            }
        }
        /// <summary>
        /// Count all input weights
        /// Include bias output weights
        /// Doesn't include bias input weights
        /// </summary>
        public int InputWeightsCount => (topology[0] + 1) * topology[1];
        /// <summary>
        /// Count all output weights
        /// Include bias output weights
        /// Doesn't include bias input weights
        /// </summary>
        public int OutputWeightsCount => (topology[topology.Length - 2] + 1) * topology[topology.Length - 1];
        /// <summary>
        /// Count all hidden weights
        /// Include biases output weights
        /// Doesn't include biases input weights
        /// </summary>
        public int HiddenWeightsCount
        {
            get
            {
                int sum = 0, length = topology.Length - 2;
                for (int counter = 1; counter < length; counter++)
                    sum += (topology[counter] + 1) * topology[counter + 1];
                return sum;
            }
        }
        /// <summary>
        /// Returns count of selected weights layer
        /// Include bias output weights
        /// Doesn't include bias input weights
        /// </summary>
        public int GetLayerWeightsCount(int layer) => (topology[layer] + 1) * topology[layer + 1];
        /// <summary>
        /// Returns count of selected neuron layer
        /// Doesn't include bias neuron
        /// </summary>
        public int GetLayerNeuronCount(int layer) => topology[layer];
        #endregion

        #region Constructors
        public NeuralNetwork(int[] topology)
        {
            this.topology = topology;
            int length = topology.Length - 1;

            weights = new float[length][];
            for (int i = 0; i < length; i++)
                weights[i] = new float[GetLayerWeightsCount(i)];
        }
        public NeuralNetwork(int[] topology, float[] weights)
        {
            this.topology = topology;

            var counter = 0;
            var lengthX = topology.Length - 1;
            this.weights = new float[lengthX][];
            
            for (int i = 0; i < lengthX; i++)
            {
                var lengthY = GetLayerWeightsCount(i);
                this.weights[i] = new float[lengthY];
                
                for (int j = 0; j < lengthY; j++)
                {
                    this.weights[i][j] = weights[counter++];
                }
            }
        }
        public NeuralNetwork(int[] topology, float[][] weights)
        {
            this.topology = topology;
            this.weights = weights;
        }
        #endregion

        #region Forward
        /// <summary>
        /// Main method of neural network
        /// Convert input layer to output layer through Forward Pass
        /// </summary>
        /// <param name="inputs">Input neuron data</param>
        /// <param name="activationType">Activation function type in standard functions</param>
        /// <returns>Output layer</returns>
        public float[] Forward(float[] inputs, ActivationType activationType = ActivationType.Sigmoid)
        {
            return Forward(inputs, Activations.Get(activationType));
        }
        /// <summary>
        /// Main method of neural network
        /// Convert input layer to output layer through Forward Pass
        /// </summary>
        /// <param name="inputs">Input neuron data</param>
        /// <param name="activator">Activation function, which normalize every neuron for next iteration</param>
        /// <returns>Output layer</returns>
        public float[] Forward(float[] inputs, ActivationCalculate activator)
        {
            var length = weights.Length;
            var inputCount = topology[0];

            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                var outputs = new float[outputCount];
                var counter = 0;

                for (int i = 0; i < outputCount; i++)
                {
                    var tempSum = 0f;
                    
                    for (int j = 0; j < inputCount; j++)
                    {
                        tempSum += inputs[j] * weights[outputCounter][counter++];
                    }
                    
                    tempSum += weights[outputCounter][counter++];
                    
                    outputs[i] = tempSum;
                }
                
                inputCount = outputCount;

                for (int i = 0; i < outputCount; i++)
                {
                    outputs[i] = activator.Invoke(outputs[i]);
                }

                inputs = outputs;
            }
            
            return inputs;
        }
        /// <summary>
        /// Main method of neural network, but returns data about all neurons in unnormalized format
        /// Convert input layer to output layer through Forward Pass
        /// </summary>
        /// <param name="snapshot">Where neuron data will be saving</param>
        /// <param name="inputs">Input neuron data</param>
        /// <param name="activationType">Activation function type in standard functions</param>
        /// <returns>All neuron layers (without bias)</returns>
        public void ForwardWithSnapshot(NetworkSnapshot snapshot, float[] inputs, ActivationType activationType = ActivationType.Sigmoid)
        {
            ForwardWithSnapshot(snapshot, inputs, Activations.Get(activationType));
        }
        /// <summary>
        /// Main method of neural network, but returns data about all neurons in unnormalized format
        /// Convert input layer to output layer through Forward Pass
        /// </summary>
        /// <param name="snapshot">Where neuron data will be saving</param>
        /// <param name="inputs">Input neuron data</param>
        /// <param name="activator">Activation function, which normalize every neuron for next iteration</param>
        /// <returns>All neuron layers (without bias)</returns>
        public void ForwardWithSnapshot(NetworkSnapshot snapshot, float[] inputs, ActivationCalculate activator)
        {
            var length = weights.Length;
            var inputCount = topology[0];
            
            snapshot[0] = inputs;
            
            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    inputs[i] = activator.Invoke(inputs[i]);
                }
                
                var outputCount = topology[outputCounter + 1];
                var outputs = new float[outputCount];
                var counter = 0;
                
                for (int i = 0; i < outputCount; i++)
                {
                    var tempSum = 0f;
                    
                    for (int j = 0; j < inputCount; j++)
                    {
                        tempSum += inputs[j] * weights[outputCounter][counter++];
                    }
                    
                    tempSum += weights[outputCounter][counter++];
                    
                    outputs[i] = tempSum;
                }
                
                inputCount = outputCount;
                inputs = outputs;
                
                snapshot[outputCounter + 1] = inputs;
            }
        }
        /// <summary>
        /// Main method of neural network, but returns data about all neurons in normalized format
        /// Convert input layer to output layer through Forward Pass
        /// </summary>
        /// <param name="snapshot">Where neuron data will be saving</param>
        /// <param name="inputs">Input neuron data</param>
        /// <param name="activationType">Activation function type in standard functions</param>
        /// <returns>All neuron layers (without bias)</returns>
        public void ForwardWithSnapshotNormalized(NetworkSnapshot snapshot, float[] inputs, ActivationType activationType = ActivationType.Sigmoid)
        {
            ForwardWithSnapshot(snapshot, inputs, Activations.Get(activationType));
        }
        /// <summary>
        /// Main method of neural network, but returns data about all neurons in normalized format
        /// Convert input layer to output layer through Forward Pass
        /// </summary>
        /// <param name="snapshot">Where neuron data will be saving</param>
        /// <param name="inputs">Input neuron data</param>
        /// <param name="activator">Activation function, which normalize every neuron for next iteration</param>
        /// <returns>All neuron layers (without bias)</returns>
        public void ForwardWithSnapshotNormalized(NetworkSnapshot snapshot, float[] inputs, ActivationCalculate activator)
        {
            var length = weights.Length;
            var inputCount = topology[0];
            
            for (int i = 0; i < inputCount; i++)
            {
                inputs[i] = activator.Invoke(inputs[i]);
            }
            
            snapshot[0] = inputs;
            
            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                var outputs = new float[outputCount];
                var counter = 0;
                
                for (int i = 0; i < outputCount; i++)
                {
                    var tempSum = 0f;
                    
                    for (int j = 0; j < inputCount; j++)
                    {
                        tempSum += inputs[j] * weights[outputCounter][counter++];
                    }
                    
                    tempSum += weights[outputCounter][counter++];
                    
                    outputs[i] = tempSum;
                }
                
                inputCount = outputCount;
                inputs = outputs;
                
                for (int i = 0; i < inputCount; i++)
                {
                    inputs[i] = activator.Invoke(inputs[i]);
                }
                
                snapshot[outputCounter + 1] = inputs;
            }
        }
        #endregion

        #region Randomize Weights
        /// <summary>
        /// This method set random weights in all NN
        /// Randomize from 0 [inclusive] to 1 [inclusive]
        /// Using Unity Random
        /// </summary>
        public void RandomizeWeights()
        {
            var length = weights.Length;
            var inputCount = topology[0];

            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                var count = (inputCount + 1) * outputCount;
                
                for (int i = 0; i < count; i++)
                {
                    weights[outputCounter][i] = UnityRandom.value;
                }

                inputCount = outputCount;
            }
        }
        /// <summary>
        /// This method set random weights in all NN
        /// Using Unity Random
        /// </summary>
        /// <param name="min">Minimum border of range</param>
        /// <param name="max">Maximum border of range</param>
        public void RandomizeWeights(float min, float max)
        {
            var length = weights.Length;
            var inputCount = topology[0];

            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                var count = (inputCount + 1) * outputCount;
                
                for (int i = 0; i < count; i++)
                {
                    weights[outputCounter][i] = UnityRandom.Range(min, max);
                }

                inputCount = outputCount;
            }
        }
        /// <summary>
        /// This method set random weights in all NN
        /// Using System Random
        /// </summary>
        /// <param name="min">Minimum border of range</param>
        /// <param name="max">Maximum border of range</param>
        /// <param name="random">Custom system random class</param>
        public void RandomizeWeights(float min, float max, SystemRandom random)
        {
            var length = weights.Length;
            var range = max - min;
            var inputCount = topology[0];

            for (int outputCounter = 0; outputCounter < length; outputCounter++)
            {
                var outputCount = topology[outputCounter + 1];
                var count = (inputCount + 1) * outputCount;
                
                for (int i = 0; i < count; i++)
                {
                    weights[outputCounter][i] = (float)random.NextDouble() * range - min;
                }

                inputCount = outputCount;
            }
        }
        #endregion

        #region Delegate Get
        /// <summary>
        /// Returns delegate with random weight, through which you can get current weight
        /// Include biases weights
        /// </summary>
        /// <returns>Delegate, through which you can get current weight</returns>
        public WeightGet GetRandomWeightGet()
        {
            var randomLayer = UnityRandom.Range(0, weights.Length);
            var randomInputWeight = UnityRandom.Range(0, topology[randomLayer] + 1);
            var randomOutputWeight = UnityRandom.Range(0, topology[randomLayer + 1]);
            return GetWeightGet(randomLayer, randomInputWeight, randomOutputWeight);
        }
        /// <summary>
        /// Returns delegate with random weight, through which you can get current weight
        /// Include biases weights
        /// </summary>
        /// <param name="random">Custom system random class</param>
        /// <returns>Delegate, through which you can get current weight</returns>
        public WeightGet GetRandomWeightGet(SystemRandom random)
        {
            var randomLayer = random.Next(0, weights.Length);
            var randomInputWeight = random.Next(0, topology[randomLayer] + 1);
            var randomOutputWeight = random.Next(0, topology[randomLayer + 1]);
            return GetWeightGet(randomLayer, randomInputWeight, randomOutputWeight);
        }
        /// <summary>
        /// Returns delegate with random weight, through which you can get current weight
        /// Include biases weights
        /// </summary>
        /// <param name="layer">Custom system random class</param>
        /// <param name="inputWeight">Custom system random class</param>
        /// <param name="outputWeight">Custom system random class</param>
        /// <returns>Delegate, through which you can get current weight</returns>
        public WeightGet GetWeightGet(int layer, int inputWeight, int outputWeight)
        {
            return () => this[layer, inputWeight, outputWeight];
        }
        /// <summary>
        /// Returns delegate with random weight, through which you can get current weight
        /// Include biases weights
        /// </summary>
        /// <param name="layer">Custom system random class</param>
        /// <param name="weight">Custom system random class</param>
        /// <returns>Delegate, through which you can get current weight</returns>
        public WeightGet GetWeightGet(int layer, int weight)
        {
            return () => this[layer, weight];
        }
        #endregion

        #region Delegate Set
        /// <summary>
        /// Returns delegate with random weight, through which you can set current weight
        /// Include biases weights
        /// </summary>
        /// <returns>Delegate, through which you can set current weight</returns>
        public WeightSet GetRandomWeightSet()
        {
            var randomLayer = UnityRandom.Range(0, weights.Length);
            var randomWeight = UnityRandom.Range(0, GetLayerWeightsCount(randomLayer));
            return GetWeightSet(randomLayer, randomWeight);
        }
        /// <summary>
        /// Returns delegate with random weight, through which you can set current weight
        /// Include biases weights
        /// </summary>
        /// <param name="random">Custom system random class</param>
        /// <returns>Delegate, through which you can set current weight</returns>
        public WeightSet GetRandomWeightSet(SystemRandom random)
        {
            var randomLayer = random.Next(0, weights.Length);
            var randomWeight = random.Next(0, GetLayerWeightsCount(randomLayer));
            return GetWeightSet(randomLayer, randomWeight);
        }
        /// <summary>
        /// Returns delegate with random weight, through which you can set current weight
        /// Include biases weights
        /// </summary>
        /// <param name="layer">Custom system random class</param>
        /// <param name="inputWeight">Custom system random class</param>
        /// <param name="outputWeight">Custom system random class</param>
        /// <returns>Delegate, through which you can set current weight</returns>
        public WeightSet GetWeightSet(int layer, int inputWeight, int outputWeight)
        {
            return value => this[layer, inputWeight, outputWeight] = value;
        }
        /// <summary>
        /// Returns delegate with random weight, through which you can set current weight
        /// Include biases weights
        /// </summary>
        /// <param name="layer">Custom system random class</param>
        /// <param name="weight">Custom system random class</param>
        /// <returns>Delegate, through which you can set current weight</returns>
        public WeightSet GetWeightSet(int layer, int weight)
        {
            return value => this[layer, weight] = value;
        }
        #endregion
        
        /// <summary>
        /// Additions all delta values with their weights
        /// Using for training network
        /// </summary>
        /// <param name="weightsDelta"></param>
        public void ApplyWeightsDelta(float[][] weightsDelta)
        {
            var length = weights.Length;
            
            for (int i = 0; i < length; i++)
            {
                var count = GetLayerWeightsCount(i);

                for (int j = 0; j < count; j++)
                {
                    weights[i][j] += weightsDelta[i][j];
                }
            }
        }

        /// <summary>
        /// Copy this class to new class
        /// </summary>
        /// <returns>Delegate, through which you can set current weight</returns>
        public NeuralNetwork Copy()
        {
            return new NeuralNetwork(topology, weights);
        }
    }
}
