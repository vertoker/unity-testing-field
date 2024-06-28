using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMP_Text : MonoBehaviour, ILocalize
    {
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            LocalizeBehaviour.Add(this);
        }

        public void OnLocalize(string text)
        {
            _text.text = text;
        }
    }
}