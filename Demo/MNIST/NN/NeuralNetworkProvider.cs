using NeuralNetworkPipeline.Saver;
using UnityEngine;

namespace NeuralNetworkPipeline.Demo.MNIST
{
    public class NeuralNetworkProvider : MonoBehaviour
    {
        [SerializeField] private NeuralNetworkSaverUnity saver;

        private NeuralNetwork nn;

        public NeuralNetwork NN => nn;

        private void Awake()
        {
            nn = saver.Load();
        }
    }
}
