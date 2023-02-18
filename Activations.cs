using System;

namespace Bobby.NN
{
    public delegate float ActivationCalculate(float value);
    
    public static class Activations
    {
        public static ActivationCalculate Get(ActivationType type)
        {
            return type switch
            {
                ActivationType.Linear => Linear,
                ActivationType.Clamp => Clamp,
                ActivationType.Sigmoid => Sigmoid,
                ActivationType.ReLu => ReLu,
                
                ActivationType.Hyperbolic => Hyperbolic,
                ActivationType.ClampNegativeInclude => ClampNegativeInclude,
                ActivationType.ReLuNegativeInclude => ReLuNegativeInclude,
                
                ActivationType.BinaryBegin => BinaryBegin,
                ActivationType.BinaryMiddle => BinaryMiddle,
                ActivationType.BinaryEnd => BinaryEnd,
                ActivationType.BinaryBeginNegativeInclude => BinaryBeginNegativeInclude,
                ActivationType.BinaryMiddleNegativeInclude => BinaryMiddleNegativeInclude,
                ActivationType.BinaryEndNegativeInclude => BinaryEndNegativeInclude,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static float[] NormalizeNeurons(float[] neurons, ActivationType type)
        {
            return NormalizeNeurons(neurons, Get(type));
        }
        public static float[] NormalizeNeurons(float[] neurons, ActivationCalculate activator)
        {
            var length = neurons.Length;
            for (int i = 0; i < length; i++)
                neurons[i] = activator.Invoke(neurons[i]);
            return neurons;
        }

        /// <summary>
        /// No changes
        /// </summary>
        public static float Linear(float value)
        {
            return value;
        }
        
        /// <summary>
        /// Clamp from 0 to 1
        /// </summary>
        public static float Clamp(float value)
        {
            return value > 1 ? 1 : value < 0 ? 0 : value;
        }
        
        /// <summary>
        /// Clamp from -1 to 1
        /// </summary>
        public static float ClampNegativeInclude(float value)
        {
            return value > 1 ? 1 : value < -1 ? -1 : value;
        }
        
        /// <summary>
        /// Clamp from 0 to 1 using exp, called "logic function"
        /// </summary>
        public static float Sigmoid(float value)
        {
            return 1.0f / (1.0f + MathF.Exp(-value));
        }
        
        /// <summary>
        /// Clamp from -1 to 1 using exp, called "hyperbolic tangent"
        /// </summary>
        public static float Hyperbolic(float value)
        {
            return MathF.Tanh(value);
        }
        
        /// <summary>
        /// Clamp everything lower 0 to 0
        /// </summary>
        public static float ReLu(float value)
        {
            return value < 0 ? 0 : value;
        }
        
        /// <summary>
        /// Clamp everything lower -1 to -1
        /// </summary>
        public static float ReLuNegativeInclude(float value)
        {
            return value < -1 ? -1 : value;
        }

        #region Binary from 0 to 1
        /// <summary>
        /// Constraint from 0 to 1 in start
        /// 0.0 => 1
        /// 0.5 => 1
        /// 1.0 => 1
        /// </summary>
        public static float BinaryBegin(float value)
        {
            return value < 0 ? 0 : 1;
        }
        /// <summary>
        /// Constraint from 0 to 1 in start
        /// 0.0 => 0
        /// 0.5 => 1
        /// 1.0 => 1
        /// </summary>
        public static float BinaryMiddle(float value)
        {
            return value < 0.5f ? 0 : 1;
        }
        /// <summary>
        /// Constraint from 0 to 1 in start
        /// 0.0 => 0
        /// 0.5 => 0
        /// 1.0 => 1
        /// </summary>
        public static float BinaryEnd(float value)
        {
            return value < 1 ? 0 : 1;
        }
        #endregion
        
        #region Binary from -1 to 1
        /// <summary>
        /// Constraint from -1 to 1 in start
        /// -1 =>  1
        ///  0 =>  1
        ///  1 =>  1
        /// </summary>
        public static float BinaryBeginNegativeInclude(float value)
        {
            return value < -1 ? -1 : 1;
        }
        /// <summary>
        /// Constraint from -1 to 1 in start
        /// -1 => -1
        ///  0 =>  1
        ///  1 =>  1
        /// </summary>
        public static float BinaryMiddleNegativeInclude(float value)
        {
            return value < 0f ? -1 : 1;
        }
        /// <summary>
        /// Constraint from -1 to 1 in start
        /// -1 => -1
        ///  0 => -1
        ///  1 =>  1
        /// </summary>
        public static float BinaryEndNegativeInclude(float value)
        {
            return value < 1 ? -1 : 1;
        }
        #endregion
    }
}