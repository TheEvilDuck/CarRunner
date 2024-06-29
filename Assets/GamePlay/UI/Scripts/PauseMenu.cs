using MainMenu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _mainButtonsMenu;
        [SerializeField] private GameObject _settingsMenu;

        public UnityEvent ResumeButtonPressed => _resumeButton.onClick;

        private void OnEnable() 
        {
            _backButton.onClick.AddListener(OnBackButtonPressed);
            _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        }

        private void OnDisable() 
        {
            _backButton.onClick.RemoveListener(OnBackButtonPressed);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
        }

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);

        private void OnSettingsButtonPressed()
        {
            _mainButtonsMenu.SetActive(false);
            _settingsMenu.SetActive(true);
        }

        private void OnBackButtonPressed()
        {
            _mainButtonsMenu.SetActive(true);
            _settingsMenu.SetActive(false);
        }
    }
}
