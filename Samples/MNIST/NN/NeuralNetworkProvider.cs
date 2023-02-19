using System;
using NN.Core;
using NN.Saver;
using UnityEngine;

namespace NN.Samples.MNIST.NN
{
    public class NeuralNetworkProvider : MonoBehaviour
    {
        [SerializeField] private NeuralNetworkSaverUnity saver;

        private NeuralNetwork nn;

        private void Start()
        {
            nn = saver.Load();
        }
    }
}
