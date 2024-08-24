using System;

namespace Services.Localization
{
    public interface ILocalizationService
    {
        public event Action languageChanged;
        public string CurrentLanguage {get;}
        public void SetLanguage(string language);
        public string GetText(string textId);
    }
}
