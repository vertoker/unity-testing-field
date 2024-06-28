using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Localization 
{
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour, ILocalize
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            LocalizeBehaviour.Add(this);
        }

        public void OnLocalize(string text)
        {
            _text.text = text;
        }
    }
}