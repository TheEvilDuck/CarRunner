using System.Collections;
using Common.CoroutineCallbacks;
using Services.PlayerData.Core;
using Services.PlayerData.Core.Language;
using UnityEngine;

namespace Services.PlayerData.Implementation.PlayerPrefsData
{
    public class PlayerPrefsLanguageDataProvider: IDataProvider<ILanguageData>
    {
        private const string LANGUAGE_KEY = "PLAYERPREFS_LANGUAGE";
        
        public IEnumerator Load(DataLoadCallback<ILanguageData> callback)
        {
            if (!PlayerPrefs.HasKey(LANGUAGE_KEY))
            {
                callback?.Invoke(false, null);
                yield break;
            }
            
            string language = PlayerPrefs.GetString(LANGUAGE_KEY);
            callback?.Invoke(true, new LanguageData(language));
            yield break;
        }

        public IEnumerator Save(ILanguageData data, SuccessCallback callback)
        {
            PlayerPrefs.SetString(LANGUAGE_KEY, data.Language);
            callback?.Invoke(true);
            yield break;
        }
        
        private class LanguageData: ILanguageData
        {
            public string Language { get; }
            
            public LanguageData(string language)
            {
                Language = language;
            }
        }
    }
}