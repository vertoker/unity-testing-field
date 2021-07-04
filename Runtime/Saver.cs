using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SerializationSaver
{
    public static class Saver
    {
        private static Dictionary<TypeSaver, string> _formats = new Dictionary<TypeSaver, string>()
        { { TypeSaver.Binary, ".dat" }, { TypeSaver.XML, ".xml" }, { TypeSaver.Json, ".json"  } };
        private static TypeSaver _typeSaver = TypeSaver.Binary;
        private static ISaver _saver = new BinarySaver();

        public static string GetFormat(TypeSaver saver)
        {
            return _formats[saver];
        }
        public static TypeSaver GetTypeSaver()
        {
            return _typeSaver;
        }
        public static void SetSaver(TypeSaver saver)
        {
            _typeSaver = saver;
            switch (_typeSaver)
            {
                case TypeSaver.Binary:
                    _saver = new BinarySaver();
                    break;
                case TypeSaver.XML:
                    _saver = new XMLSaver();
                    break;
                case TypeSaver.Json:
                    _saver = new JsonSaver();
                    break;
            }
        }

        public static void Save<T>(T data, params string[] paths)
        {
            _saver.Save(data, paths);
        }
        public static void Save<T>(T data, string path)
        {
            _saver.Save(data, path);
        }
        public static T Load<T>(params string[] paths)
        {
            return _saver.Load<T>(paths);
        }
        public static T Load<T>(string path)
        {
            return _saver.Load<T>(path);
        }
    }
}