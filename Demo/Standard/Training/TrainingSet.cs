using UnityEngine;

namespace NeuralNetworkPipeline.Demo.Standard.Training
{
    [CreateAssetMenu(fileName = "Set", menuName = "NeuralNetwork/Training Set", order = 0)]
    public class TrainingSet : ScriptableObject
    {
        [SerializeField] private double[] _input;
        [SerializeField] private double[] _output;

        public double[] InputLayer => _input;
        public double[] OutputLayer => _output;

        public double ErrorCalculate(double[] output, ErrorCalculate calculate)
        {
            return calculate.Invoke(_output, output);
        }
    }
}