using UnityEngine;

namespace Localization
{
    public class LocalizationData : ScriptableObject
    {
        [SerializeField] private string _eng;
        [SerializeField] private string _rus;

        public string GetString(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return _eng;
                case Language.Russian:
                    return _rus;
            }
            return string.Empty;
        }
    }
}