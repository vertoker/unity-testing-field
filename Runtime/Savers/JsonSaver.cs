using UnityEngine;
using System.IO;
using System;

namespace Game.SerializationSaver
{
    /// <summary>
    /// Use to save small data (500- objects)
    /// </summary>
    public static class JsonSaver
    {
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

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
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

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
    }
}