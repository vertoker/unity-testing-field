using Game.SerializationSaver;
using UnityEngine;
using System;

public class LoadTest : MonoBehaviour
{
    [SerializeField] private TypeSaver _typeLoad;
    [SerializeField] private DemoClass _demo;

    void Start()
    {
        var format = _typeLoad switch
        {
            TypeSaver.Binary => ".dat",
            TypeSaver.Json => ".json",
            TypeSaver.XML => ".xml",
            _ => string.Empty,
        };
        var name = DateTime.UtcNow.ToString("ss-mm-hh-dd-MM-yyyy") + format;
        var path = SaverStatic.PathCombine(Application.dataPath, "Saves", name);

        switch (_typeLoad)
        {
            case TypeSaver.Binary: _demo = BinarySaver.Load<DemoClass>(path); break;
            case TypeSaver.Json: _demo = JsonSaver.Load<DemoClass>(path); break;
            case TypeSaver.XML: _demo = XMLSaver.Load<DemoClass>(path); break;
        };
    }
}
