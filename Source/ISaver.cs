namespace SerializationSaver
{
    public interface ISaver
    {
        public void Save<T>(T data, params string[] paths);
        public void Save<T>(T data, string path);
        public T Load<T>(params string[] paths);
        public T Load<T>(string path);
    }
}