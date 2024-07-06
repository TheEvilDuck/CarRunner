using Common.UI.UIAnimations;
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
        [SerializeField] private UIAnimatorSequence _uIanimatorSequenceButtons;
        [SerializeField] private UIAnimatorSequence _uIanimatorSequenceMenu;

        public UnityEvent ResumeButtonPressed => _resumeButton.onClick;

        private void OnEnable() 
        {
            _mainButtonsMenu.SetActive(true);
            _uIanimatorSequenceMenu.StartSequence();
            _uIanimatorSequenceButtons.StartSequence();
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
            _settingsMenu.SetActive(false);
            _mainButtonsMenu.SetActive(true);
            _uIanimatorSequenceButtons.StartSequence();
        }
    }
}
