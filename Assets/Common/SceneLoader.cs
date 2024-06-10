using UnityEngine.SceneManagement;

namespace Common
{
    public class SceneLoader
    {
        private const int _mainMenu = 0;
        private const int _gameplay = 1;

        public void LoadMainMenu() => SceneManager.LoadScene(_mainMenu);
        public void LoadGameplay() => SceneManager.LoadScene(_gameplay);
        public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

