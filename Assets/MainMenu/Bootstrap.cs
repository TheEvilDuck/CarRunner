using Common;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private SettingsMenu _settingsMenu;
        [SerializeField] private GameSettings _gameSettings;
        private MainMenuMediator _mainMenuMediator;
        private SettingsMediator _settingsMediator;
        private SceneLoader _sceneLoader;
        private PlayerData _playerData;

        private void Awake()
        {
            _sceneLoader = new SceneLoader();
            _playerData = new PlayerData();
            _mainMenuMediator = new MainMenuMediator(_mainMenuView, _playerData, _sceneLoader);
            _settingsMediator = new SettingsMediator(_gameSettings, _settingsMenu);
        }

        private void OnDestroy()
        {
            _mainMenuMediator.Dispose();
            _settingsMediator.Dispose();
        }
    }
}


