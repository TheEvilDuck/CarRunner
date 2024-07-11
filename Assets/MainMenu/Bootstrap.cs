using Common;
using Common.Data;
using Common.Mediators;
using Common.Sound;
using EntryPoint;
using MainMenu.LevelSelection;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviourBootstrap
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private SettingsMenu _settingsMenu;
        [SerializeField] private LevelSelector _levelSelector;
        [SerializeField] private NotEnoughMoneyPopup _notEnoughMoneyPopup;
        private GameSettings _gameSettings;
        private MainMenuMediator _mainMenuMediator;
        private SettingsMediator _settingsMediator;
        private SettingsAndSoundMediator _settingsAndSoundMediator;
        private void Start() 
        {
            _settingsMenu.Init(_gameSettings);
            _settingsAndSoundMediator = new SettingsAndSoundMediator(_sceneContext);
            _sceneContext.Get<SoundController>().Play(SoundID.MainMenuMusic, true);
        }

        private void OnDestroy()
        {
            _gameSettings.SaveSettings();
            _mainMenuMediator.Dispose();
            _settingsMediator.Dispose();
            _settingsAndSoundMediator.Dispose();

            _sceneContext.Get<SoundController>().Stop(SoundID.MainMenuMusic);
        }

        protected override void Setup()
        {
            _sceneContext.Register(_mainMenuView);
            _sceneContext.Register(_settingsMenu);
            _sceneContext.Register(_levelSelector);
            _sceneContext.Register(_notEnoughMoneyPopup);

            _gameSettings = _sceneContext.Get<GameSettings>();

            _mainMenuMediator = new MainMenuMediator(_sceneContext);
            _settingsMediator = new SettingsMediator(_sceneContext);

            IPlayerData playerData = _sceneContext.Get<IPlayerData>();

            _levelSelector.Init(playerData.PassedLevels, playerData.AvailableLevels);
        }
    }
}


