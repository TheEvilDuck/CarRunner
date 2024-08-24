using System;
using Common.Data;
using DI;
using MainMenu.LanguageSelection;
using Services.Localization;

namespace MainMenu
{
    public class LanguageMediator : IDisposable
    {
        private readonly IPlayerData _playerData;
        private readonly ILocalizationService _localizationService;
        private readonly LanguageSelectorMenu _languageSelectorMenu;
        public LanguageMediator(DIContainer sceneContext)
        {
            _playerData = sceneContext.Get<IPlayerData>();
            _localizationService = sceneContext.Get<ILocalizationService>();
            _languageSelectorMenu = sceneContext.Get<LanguageSelectorMenu>();

            _localizationService.languageChanged += OnLanguageChanged;
            _languageSelectorMenu.languageChanged += OnLanguageSelected;
        }
        public void Dispose()
        {
            _localizationService.languageChanged -= OnLanguageChanged;
            _languageSelectorMenu.languageChanged -= OnLanguageSelected;
        }

        private void OnLanguageChanged()
        {
            _playerData.SaveLanguage(_localizationService.CurrentLanguage);
        }

        private void OnLanguageSelected(string language)
        {
            _localizationService.SetLanguage(language);
        }
    }
}
