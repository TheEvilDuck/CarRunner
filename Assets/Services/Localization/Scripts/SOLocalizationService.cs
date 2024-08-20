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

        public string GetText(string textId)
        {
            if (_currentLanguage == string.Empty)
                _currentLanguage = _defaultLanguage.LanguageId;

            int localizationDataIndex = _localizationDatas.FindIndex((x) => x.textId == textId);

            if (localizationDataIndex == -1)
            {
                throw new ArgumentException($"Localization service {name} has no implementation of localizable: {textId}");
            }

            int localizationDataElementIndex = _localizationDatas.Find((x) => x.textId == textId).localizationDataElements.FindIndex((x) => x.language.LanguageId == _currentLanguage);

            if (localizationDataElementIndex == -1)
            {
                throw new ArgumentException($"Localization service {name} has no implementation of localizable: {textId} for language: {_currentLanguage}");
            }

            return _localizationDatas.Find((x) => x.textId == textId).localizationDataElements.Find((x) => x.language.LanguageId == _currentLanguage).tranlation;
        }

        public void SetLanguage(string language)
        {
            if (language == string.Empty)
                _currentLanguage = _defaultLanguage.LanguageId;

            _currentLanguage = language;
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
            public string tranlation;
        }
    }
}
