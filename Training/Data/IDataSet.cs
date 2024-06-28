namespace NeuralNetworkPipeline.Training
{
    public interface IDataSet
    {
        public float[] GetInput();
        public float[] GetOutput();
    }
}
