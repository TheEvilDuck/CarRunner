using System;
using System.Collections.Generic;

namespace Services.Localization
{
    public class Localizator: IDisposable
    {
        private readonly ILocalizationService _localizationService;
        private readonly List<ILocalizable> _localizables;

        public Localizator(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            _localizables = new List<ILocalizable>();

            _localizationService.languageChanged += TranslateAll;
            TranslateAll();
        }

        public void RegisterLocalizable(ILocalizable localizable)
        {
            if (_localizables.Contains(localizable))
                throw new ArgumentException($"The localizable you passed is already registered!");

            localizable.updateRequested += TranslateLocalizable;
            TranslateLocalizable(localizable);
            _localizables.Add(localizable);
        }

        public void Dispose()
        {
            _localizationService.languageChanged -= TranslateAll;

            foreach (ILocalizable localizable in _localizables)
                localizable.updateRequested -= TranslateLocalizable;
        }

        private void TranslateAll()
        {   
            foreach (ILocalizable localizable in _localizables)
                TranslateLocalizable(localizable);
        }

        private void TranslateLocalizable(ILocalizable localizable) => localizable.UpdateText(_localizationService.GetText(localizable.TextId));
    }
}
