using System;
using System.Collections.Generic;
using Services.PlayerData.Core.Language;

namespace Services.Localization.Scripts
{
    public class Localizator: IDisposable
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly List<ILocalizable> _localizables;

        public Localizator(
            ILocalizationService localizationService, 
            ILanguageService languageService)
        {
            _localizationService = localizationService;
            _languageService = languageService;
            _localizables = new List<ILocalizable>();

            _languageService.Language.changed += OnLanguageChanged;
            TranslateAll();
        }

        public void RegisterLocalizable(ILocalizable localizable, bool instantTranslate = true)
        {
            if (_localizables.Contains(localizable))
                throw new ArgumentException($"The localizable you passed is already registered!");

            localizable.updateRequested += TranslateLocalizable;
            
            if (instantTranslate)
                TranslateLocalizable(localizable);

            _localizables.Add(localizable);
        }

        public void Dispose()
        {
            _languageService.Language.changed -= OnLanguageChanged;

            foreach (ILocalizable localizable in _localizables)
                localizable.updateRequested -= TranslateLocalizable;
        }

        private void TranslateAll()
        {   
            for (int i = _localizables.Count - 1; i >= 0; i--)
            {
                if (_localizables[i] == null)
                    _localizables.RemoveAt(i);
                else
                    TranslateLocalizable(_localizables[i]);
            }
        }

        private void TranslateLocalizable(ILocalizable localizable)
        {
            string localizedText 
                = _localizationService.GetText(_languageService.Language.Value, localizable.TextId);
            
            localizable.UpdateText(localizedText);
        }

        private void OnLanguageChanged(string language) => TranslateAll();
    }
}
