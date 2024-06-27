using System;
using UnityEngine;

namespace Calculator.Demo
{
    [ExecuteInEditMode]
    public class CalculatorDemo : MonoBehaviour
    {
        [Header("Expression")] 
        [SerializeField] private string expression;

        [Header("Answer")]
        [SerializeField] private double answer;
        
        private void Update()
        {
            answer = ExpressionCalculator.Calculate(expression);
        }
    }
}
