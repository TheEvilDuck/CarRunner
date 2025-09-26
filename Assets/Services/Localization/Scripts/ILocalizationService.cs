using System;

namespace Services.Localization.Scripts
{
    public interface ILocalizationService
    {
        public string GetText(string language, string textId);
    }
}
