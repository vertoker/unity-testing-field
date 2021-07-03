using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using System;

namespace Game.SerializationSaver
{
    /// <summary>
    /// Use to save big data (1500+ objects)
    /// </summary>
    public static class XMLSaver
    {
        private static readonly Type _typeSerializer = null;
        private static XmlSerializer _serializer;

        public static void Save<T>(T data, params string[] paths)
        {
            var path = SaverStatic.PathCombine(paths);
            Save(data, path);
        }
        public static void Save<T>(T data, string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            Type type = typeof(T);
            if (_typeSerializer != type)
                _serializer = new XmlSerializer(type);

            TextWriter writer = new StreamWriter(path);
            _serializer.Serialize(writer, data);
        }

        public static T Load<T>(params string[] paths)
        {
            var path = SaverStatic.PathCombine(paths);
            return Load<T>(path);
        }
        public static T Load<T>(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError(path + SaverStatic.LOAD_EXCEPTION);
                return (T)SaverStatic.EMPTY;
            }
            Type type = typeof(T);
            if (_typeSerializer != type)
                _serializer = new XmlSerializer(type);

            TextReader reader = new StreamReader(path);
            object data = _serializer.Deserialize(reader);
            return (T)data;
        }
    }
}