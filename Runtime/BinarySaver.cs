using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using System.IO;

namespace Game.SerializationSaver
{
    /// <summary>
    /// Use to save private data
    /// </summary>
    public class BinarySaver : ISaver
    {
        private readonly BinaryFormatter _formatter;

        public BinarySaver()
        {
            _formatter = GetFormatter();
        }

        private BinaryFormatter GetFormatter()
        {
            var formatter = new BinaryFormatter();
            var selector = new SurrogateSelector();
            var context = new StreamingContext(StreamingContextStates.All);

            selector.AddSurrogate(typeof(Vector2), context, new Vector2Serializer());
            selector.AddSurrogate(typeof(Vector2Int), context, new Vector2IntSerializer());
            selector.AddSurrogate(typeof(Vector3), context, new Vector3Serializer());
            selector.AddSurrogate(typeof(Vector3Int), context, new Vector3IntSerializer());
            selector.AddSurrogate(typeof(Vector4), context, new Vector4Serializer());

            selector.AddSurrogate(typeof(Color32), context, new Color32Serializer());
            selector.AddSurrogate(typeof(Color), context, new ColorSerializer());
            selector.AddSurrogate(typeof(Quaternion), context, new QuaternionSerializer());
            selector.AddSurrogate(typeof(RectInt), context, new RectIntSerializer());
            selector.AddSurrogate(typeof(Rect), context, new RectSerializer());
            selector.AddSurrogate(typeof(Texture2D), context, new Texture2DSerializer());

            formatter.SurrogateSelector = selector;
            return formatter;
        }

        public void Save<T>(T data, params string[] paths)
        {
            var path = SaverStatic.PathCombine(paths);
            Save(data, path);
        }
        public void Save<T>(T data, string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using FileStream stream = File.Create(path);
            _formatter.Serialize(stream, data);
        }

        public T Load<T>(params string[] paths)
        {
            var path = SaverStatic.PathCombine(paths);
            return Load<T>(path);
        }
        public T Load<T>(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError(path + SaverStatic.LOAD_EXCEPTION);
                return (T)SaverStatic.EMPTY;
            }

            FileStream file = File.Open(path, FileMode.Open);
            object data = _formatter.Deserialize(file);
            return (T)data;
        }
    }
}