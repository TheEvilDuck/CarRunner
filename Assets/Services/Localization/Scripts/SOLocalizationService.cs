using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Localization
{
    [CreateAssetMenu(menuName = "Localization/New SO lozalization service", fileName = "SO localization service")]
    public class SOLocalizationService : ScriptableObject, ILocalizationService
    {
        public event Action languageChanged;
        [SerializeField] private List<LocalizationData> _localizationDatas;
        [SerializeField] private LanguageData _defaultLanguage;

        private string _currentLanguage;

        public string CurrentLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(_currentLanguage))
                    _currentLanguage = _defaultLanguage.LanguageId;

                return _currentLanguage;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _currentLanguage = value;
            }
        }

        public string GetText(string textId)
        {
            if (CurrentLanguage == string.Empty)
                CurrentLanguage = _defaultLanguage.LanguageId;

            int localizationDataIndex = _localizationDatas.FindIndex((x) => x.textId == textId);

            if (localizationDataIndex == -1)
            {
                Debug.LogError($"Localization service {name} has no implementation of localizable: {textId}");
                return textId;
            }

            int localizationDataElementIndex = _localizationDatas.Find((x) => x.textId == textId).localizationDataElements.FindIndex((x) => x.language.LanguageId == CurrentLanguage);

            if (localizationDataElementIndex == -1)
            {
                Debug.LogError($"Localization service {name} has no implementation of localizable: {textId} for language: {CurrentLanguage}");
                return textId;
            }

            return _localizationDatas.Find((x) => x.textId == textId).localizationDataElements.Find((x) => x.language.LanguageId == CurrentLanguage).translation;
        }

        public void SetLanguage(string language)
        {
            if (language == string.Empty)
                CurrentLanguage = _defaultLanguage.LanguageId;

            CurrentLanguage = language;
            languageChanged?.Invoke();
        }

        [Serializable]
        private struct LocalizationData
        {
            public string textId;
            public List<LocalizationDataElement> localizationDataElements;
        }

        [Serializable]
        private struct LocalizationDataElement
        {
            public LanguageData language;
            [TextArea(3,10)] public string translation;
        }
    }
}
