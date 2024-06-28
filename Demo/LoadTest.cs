using UnityEngine;

namespace SerializationSaver.Demo
{
    public class LoadTest : MonoBehaviour
    {
        [SerializeField] private TypeSaver _typeLoad;
        [SerializeField] private DemoClass _demo;

        public void Load(string path)
        {
            _demo = Saver.Load<DemoClass>(path);
        }
    }
}
