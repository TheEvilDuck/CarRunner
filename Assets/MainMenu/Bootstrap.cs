using Common;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private MainMenuView _mainMenuView;
        private SceneLoader _sceneLoader;
        private MainMenuMediator _mainMenuMediator;

        private void Awake()
        {
            _sceneLoader = new SceneLoader();
            _mainMenuMediator = new MainMenuMediator(_mainMenuView, _sceneLoader);
        }

        private void OnDestroy()
        {
            _mainMenuMediator.Dispose();
        }
    }
}


