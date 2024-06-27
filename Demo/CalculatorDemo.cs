using System;
using UnityEngine;

namespace Calculator.Demo
{
    public class CalculatorDemo : MonoBehaviour
    {
        [Header("Expression")] 
        [SerializeField] private string expression;

        [Header("Answer")]
        [SerializeField] private double answer;
        
        private void Update()
        {
            Debug.Log(1);
            answer = ExpressionCalculator.Calculate(expression);
        }
    }
}
