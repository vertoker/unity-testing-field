using TMPro;
using UnityEngine;

namespace NeuralNetworkPipeline.Demo.MNIST.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextParameter : MonoBehaviour
    {
        [SerializeField] private string parameterName;
        private TMP_Text self;

        private void Awake()
        {
            self = GetComponent<TMP_Text>();
        }

        public void UpdateParameter(int value)
        {
            self.text = $"{parameterName} - {value}";
        }
        public void UpdateParameter(float value)
        {
            self.text = $"{parameterName} - {value}";
        }
        public void UpdateParameter(string value)
        {
            self.text = $"{parameterName} - {value}";
        }
    }
}
