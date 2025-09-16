using UnityEngine;

namespace Common.LoadingCurtain
{
    public class LoadingCurtainService : ILoadingCurtainService
    {
        private const string LOADING_SCREEN_PATH = "Loading screen";
        
        private Canvas _screen;

        public void Show()
        {
            if (_screen == null)
            {
                _screen = Object.Instantiate(Resources.Load<Canvas>(LOADING_SCREEN_PATH));
                Object.DontDestroyOnLoad(_screen);
            }
            
            _screen.gameObject.SetActive(true);
        }
        public void Hide() => _screen.gameObject.SetActive(false);
    }
}
