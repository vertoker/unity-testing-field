using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using System;

namespace Game.SerializationSaver
{
    /// <summary>
    /// Use to save big data (1500+ objects)
    /// </summary>
    public class XMLSaver : ISaver
    {
        private Type _typeSerializer = null;
        private XmlSerializer _serializer;

        public XMLSaver()
        {

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

            Type type = typeof(T);
            if (_typeSerializer != type)
            {
                _serializer = new XmlSerializer(type);
                _typeSerializer = type;
            }

            using FileStream stream = File.Create(path);
            _serializer.Serialize(stream, data);
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

            Type type = typeof(T);
            if (_typeSerializer != type)
            {
                _serializer = new XmlSerializer(type);
                _typeSerializer = type;
            }

            FileStream file = File.Open(path, FileMode.Open);
            object data = _serializer.Deserialize(file);
            return (T)data;
        }
    }
}