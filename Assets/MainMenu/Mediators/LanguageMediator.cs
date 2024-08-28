using System;
using Common.Data;
using DI;
using MainMenu.LanguageSelection;

namespace MainMenu
{
    public class LanguageMediator : IDisposable
    {
        private readonly IPlayerData _playerData;
        private readonly LanguageSelectorMenu _languageSelectorMenu;

        public LanguageMediator(DIContainer sceneContext)
        {
            _playerData = sceneContext.Get<IPlayerData>();
            _languageSelectorMenu = sceneContext.Get<LanguageSelectorMenu>();

            _languageSelectorMenu.languageChanged += OnLanguageChanged;
        }
        public void Dispose()
        {
            _languageSelectorMenu.languageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(string language)
        {
            _playerData.SaveLanguage(language);
        }
    }
}
