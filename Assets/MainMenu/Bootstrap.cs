using System;
using System.Collections.Generic;
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
        [SerializeField] private CoinsView _coinsView;
        private GameSettings _gameSettings;
        List<IDisposable> _disposables;
        private void Start() 
        {
            _settingsMenu.Init(_gameSettings);
            var settingsAndSoundMediator = new SettingsAndSoundMediator(_sceneContext);
            _disposables.Add(settingsAndSoundMediator);
            _sceneContext.Get<SoundController>().Play(SoundID.MainMenuMusic, true);
        }

        private void OnDestroy()
        {
            _gameSettings.SaveSettings();
            
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            _disposables.Clear();

            _sceneContext.Get<SoundController>().Stop(SoundID.MainMenuMusic);
        }

        protected override void Setup()
        {
            _disposables = new List<IDisposable>();

            _sceneContext.Register(_mainMenuView);
            _sceneContext.Register(_settingsMenu);
            _sceneContext.Register(_levelSelector);
            _sceneContext.Register(_notEnoughMoneyPopup);
            _sceneContext.Register(_coinsView);

            _gameSettings = _sceneContext.Get<GameSettings>();

            var mainMenuMediator = new MainMenuMediator(_sceneContext);
            var settingsMediator = new SettingsMediator(_sceneContext);
            var coinsMediator = new CoinsMediator(_sceneContext);

            _disposables.Add(mainMenuMediator);
            _disposables.Add(settingsMediator);
            _disposables.Add(coinsMediator);

            IPlayerData playerData = _sceneContext.Get<IPlayerData>();

            _levelSelector.Init(playerData.PassedLevels, playerData.AvailableLevels);
        }
    }
}


