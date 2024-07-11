using MainMenu.LevelSelection;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [field: SerializeField] public MainButtons MainButtons {get; private set;}
        [field: SerializeField] public LevelSelector LevelSelector {get; private set;}
        [field: SerializeField] public SettingsMenu SettingsMenu { get; private set;}

        private void Awake() 
        {
            ShowMainButtons();
        }

        private void OnEnable() 
        {
            MainButtons.PlayClickedEvent.AddListener(ShowLevelSelector);
            MainButtons.SettingsClickedEvent.AddListener(ShowSettingsMenu);
            LevelSelector.BackPressed.AddListener(ShowMainButtons);
            SettingsMenu.BackPressed.AddListener(ShowMainButtons);
        }

        private void OnDisable() 
        {
            MainButtons.PlayClickedEvent.RemoveListener(ShowLevelSelector);
            MainButtons.SettingsClickedEvent.RemoveListener(ShowSettingsMenu);
            LevelSelector.BackPressed.RemoveListener(ShowMainButtons);
            SettingsMenu.BackPressed.RemoveListener(ShowMainButtons);
        }

        private void ShowMainButtons()
        {
            MainButtons.gameObject.SetActive(true);
            LevelSelector.gameObject.SetActive(false);
            SettingsMenu.gameObject.SetActive(false);
        }

        private void ShowLevelSelector()
        {
            MainButtons.gameObject.SetActive(false);
            LevelSelector.gameObject.SetActive(true);
        }

        private void ShowSettingsMenu()
        {
            MainButtons.gameObject.SetActive(false);
            SettingsMenu.gameObject.SetActive(true);
        }
    }
}

