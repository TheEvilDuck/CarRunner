using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Localization.Scripts
{
    [CreateAssetMenu(menuName = "Localization/New SO lozalization service", fileName = "SO localization service")]
    public class SOLocalizationService : ScriptableObject, ILocalizationService
    {
        [SerializeField] private List<LocalizationData> _localizationDatas;
        
        public string GetText(string language, string textId)
        {
            int localizationDataIndex = _localizationDatas.FindIndex((x) => x.textId == textId);

            if (localizationDataIndex == -1)
            {
                Debug.LogError($"Localization service {name} has no implementation of localizable: {textId}");
                return textId;
            }

            int localizationDataElementIndex = _localizationDatas.Find((x) => x.textId == textId).localizationDataElements.FindIndex((x) => x.language.LanguageId == language);

            if (localizationDataElementIndex == -1)
            {
                Debug.LogError($"Localization service {name} has no implementation of localizable: {textId} for language: {language}");
                return textId;
            }

            return _localizationDatas.Find((x) => x.textId == textId).localizationDataElements.Find((x) => x.language.LanguageId == language).translation;
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
