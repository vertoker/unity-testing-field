using System;
using UnityEngine;

namespace NeuralNetworkPipeline.Training
{
    public delegate float ErrorCalculate(float[] output, float[] ideal);

    public static class Errors
    {
        public static ErrorCalculate Get(ErrorType type)
        {
            return type switch
            {
                ErrorType.MSE => MSE,
                ErrorType.RootMSE => RootMSE,
                ErrorType.Arctan => Arctan,
                
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        /// <summary>
        /// Mean Squared Error
        /// return diff between output and ideal in error from 0 to 1
        /// </summary>
        public static float MSE(float[] output, float[] ideal)
        {
            var sum = 0f;
            var length = output.Length;
            for (int i = 0; i < length; i++)
                sum += Mathf.Pow(ideal[i] - output[i], 2);
            return sum / length;
        }

        /// <summary>
        /// Root (sqrt) Mean Squared Error
        /// return diff between output and ideal in error from 0 to 1
        /// </summary>
        public static float RootMSE(float[] output, float[] ideal)
        {
            return Mathf.Sqrt(MSE(output, ideal));
        }

        /// <summary>
        /// Mean Squared Error
        /// return diff between output and ideal in error from 0 to 1
        /// </summary>
        public static float Arctan(float[] output, float[] ideal)
        {
            var sum = 0f;
            var length = output.Length;
            for (int i = 0; i < length; i++)
                sum += Mathf.Atan(ideal[i] - output[i]);
            return sum / length;
        }
    }
}