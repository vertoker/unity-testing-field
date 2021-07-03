using Game.SerializationSaver;
using UnityEngine;
using System;

public enum TypeSaver { Binary = 1, XML = 2, Json = 3 }
public class SaveTest : MonoBehaviour
{
    [SerializeField] private TypeSaver _typeSave;
    [SerializeField] private DemoClass _demo;

    void Awake()
    {
        var format = _typeSave switch
        {
            TypeSaver.Binary => ".dat",
            TypeSaver.Json => ".json",
            TypeSaver.XML => ".xml",
            _ => string.Empty,
        };
        var name = DateTime.UtcNow.ToString("ss-mm-hh-dd-MM-yyyy") + format;
        var path = SaverStatic.PathCombine(Application.dataPath, "Saves", name);

        switch (_typeSave)
        {
            case TypeSaver.Binary: BinarySaver.Save(_demo, path); break;
            case TypeSaver.Json: JsonSaver.Save(_demo, path); break;
            case TypeSaver.XML: XMLSaver.Save(_demo, path); break;
        };
    }
}
