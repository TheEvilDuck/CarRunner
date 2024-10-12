using UnityEngine;

namespace Common
{
    public class LoadScreen
    {
        private const string LOADING_SCREEN = "Loading screen";
        private Canvas _screen;

        public GameObject Screen => _screen.gameObject;
        public LoadScreen()
        {
            _screen = MonoBehaviour.Instantiate(Resources.Load<Canvas>(LOADING_SCREEN));
            UnityEngine.Object.DontDestroyOnLoad(_screen);

            Hide();
        }

        public void Show() => _screen.gameObject.SetActive(true);
        public void Hide() => _screen.gameObject.SetActive(false);
    }
}
