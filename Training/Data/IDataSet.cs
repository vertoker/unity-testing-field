namespace NN.Training.Data
{
    public interface IDataSet
    {
        public float[] GetInput();
        public float[] GetOutput();
    }
}
