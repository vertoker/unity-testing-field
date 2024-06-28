using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NeuralNetworkPipeline.Demo.MNIST.UI
{
    public class WeightReceiver : MonoBehaviour
    {
        [SerializeField] private int width = 20;
        [SerializeField] private int height = 20;
        [SerializeField] private NeuralNetworkProvider provider;
        [SerializeField] private float timeUntilUpdate = 0.5f;
        
        [SerializeField] private UnityEvent<string> updateAnswer;
        
        [Header("Last outputs")]
        [SerializeField] private float[] outputs;
        
        private float[] inputLayer;
        private Coroutine updateWaiter;

        private void Awake()
        {
            inputLayer = new float[width * height];
        }
        public void SetPixel(int x, int y, Color color)
        {
            inputLayer[y * height + x] = GetStatePixel(color);

            if (updateWaiter != null)
                StopCoroutine(updateWaiter);
            updateAnswer.Invoke("-");
            updateWaiter = StartCoroutine(UpdateThroughForward());
        }

        public IEnumerator UpdateThroughForward()
        {
            yield return new WaitForSeconds(timeUntilUpdate);
            outputs = provider.NN.Forward(inputLayer);
            
            var (_, index) = outputs.Select((n, i) => (n, i)).Max();
            updateAnswer.Invoke(index.ToString());
        }
        
        public static float GetStatePixel(Color color)
        {
            return color == Color.white ? 0 : 1;
        }
    }
}
