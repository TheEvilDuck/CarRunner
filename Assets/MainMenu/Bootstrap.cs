using Common;
using Common.Mediators;
using Common.Sound;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private SettingsMenu _settingsMenu;
        [SerializeField] private SoundController _soundController;
        private GameSettings _gameSettings;
        private MainMenuMediator _mainMenuMediator;
        private SettingsMediator _settingsMediator;
        private SettingsAndSoundMediator _settingsAndSoundMediator;
        private SceneLoader _sceneLoader;
        private PlayerData _playerData;
        

        private void Awake()
        {
            _sceneLoader = new SceneLoader();
            _gameSettings = new GameSettings();
            _gameSettings.LoadSettings();
            _playerData = new PlayerData();
            _mainMenuMediator = new MainMenuMediator(_mainMenuView, _playerData, _sceneLoader);
            _settingsMediator = new SettingsMediator(_gameSettings, _settingsMenu);
        }

        private void Start() 
        {
            _soundController.Init();
            _settingsMenu.Init(_gameSettings);
            _settingsAndSoundMediator = new SettingsAndSoundMediator(_gameSettings, _soundController);
            _soundController.Play(SoundID.MainMenuMusic);
        }

        private void OnDestroy()
        {
            _gameSettings.SaveSettings();
            _mainMenuMediator.Dispose();
            _settingsMediator.Dispose();
            _settingsAndSoundMediator.Dispose();
        }
    }
}


