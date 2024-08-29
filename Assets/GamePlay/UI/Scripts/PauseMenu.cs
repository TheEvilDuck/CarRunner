using Common;
using Common.MenuParent;
using Common.UI;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.UI
{
    public class PauseMenu : MonoBehaviour, IPausable
    {
        
        [SerializeField] private PauseMainButtons _mainButtonsMenu;
        [SerializeField] private GameSettingsUI _settingsMenu;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        private MenuParentsManager _menuParentsManager;

        public UnityEvent ResumeButtonPressed => _mainButtonsMenu.ResumeClicked;

        private void Awake() 
        {
            _menuParentsManager = new MenuParentsManager();

            _menuParentsManager.Add(_settingsMenu);
            _menuParentsManager.Add(_mainButtonsMenu);
            
        }

        private void OnEnable() 
        {
            _settingsMenu.BackPressed.AddListener(OnBackButtonPressed);
            _mainButtonsMenu.SettingsClicked.AddListener(OnSettingsButtonPressed);
        }

        private void OnDisable() 
        {
            _settingsMenu.BackPressed.RemoveListener(OnBackButtonPressed);
            _mainButtonsMenu.SettingsClicked.RemoveListener(OnSettingsButtonPressed);
        }

        private void OnSettingsButtonPressed() => _menuParentsManager.Show(_settingsMenu);

        private void OnBackButtonPressed() => _menuParentsManager.Show(_mainButtonsMenu);

        public void Pause()
        {
            gameObject.SetActive(true);
            _menuParentsManager.Show(_mainButtonsMenu);
            _uIAnimatorSequence.StartSequence();
        }

        public void Resume()
        {
            gameObject.SetActive(false);
        }
    }
}
