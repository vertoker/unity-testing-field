namespace NeuralNetworkPipeline.Training
{
    public class BackPropagation
    {
        private NeuralNetwork network;

        public NeuralNetwork Network => network;

        public BackPropagation(NeuralNetwork network)
        {
            this.network = network;
        }

        public void Backward(NetworkSnapshot ns, float error)
        {
            
        }
    }
}
