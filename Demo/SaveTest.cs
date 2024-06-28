using System;
using UnityEngine;

namespace SerializationSaver.Demo
{
    public class SaveTest : MonoBehaviour
    {
        [SerializeField] private TypeSaver _typeSave;
        [SerializeField] private DemoClass _demo;
        [SerializeField] private LoadTest _load;

        void Awake()
        {
            Saver.SetSaver(_typeSave);
            var name = DateTime.UtcNow.ToString("ss-mm-hh-dd-MM-yyyy") + Saver.GetFormat(_typeSave);
            var path = SaverStatic.PathCombine(Application.dataPath, "Saves", name);
            Saver.Save(_demo, path);
            _load.Load(path);
        }
    }
}
