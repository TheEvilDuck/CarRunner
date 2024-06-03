using Common;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private MainMenuView _mainMenuView;
        private SceneLoader _sceneLoader;
        private MainMenuMediator _mainMenuMediator;
        private PlayerData _playerData;

        private void Awake()
        {
            _sceneLoader = new SceneLoader();
            _playerData = new PlayerData();
            _mainMenuMediator = new MainMenuMediator(_mainMenuView, _playerData, _sceneLoader);
        }

        private void OnDestroy()
        {
            _mainMenuMediator.Dispose();
        }
    }
}


