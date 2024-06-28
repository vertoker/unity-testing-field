using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public class LocalizeBehaviour : MonoBehaviour
    {
        [SerializeField] private List<ILocalize> _data = new List<ILocalize>();
        [SerializeField] private bool _localizeOnStart = true;
        private static LocalizeBehaviour _instance;
        private int _length = 0;

        private void Awake()
        {
            _instance = this;
        }

        public static void Add(ILocalize item)
        {
            _instance._data.Add(item);
            _instance._length++;
        }

        private void Start()
        {
            if (_localizeOnStart)
                Localize();
        }

        public static void Localize()
        {
            for (int i = 0; i < _instance._length; i++)
            {

            }
        }
    }
}