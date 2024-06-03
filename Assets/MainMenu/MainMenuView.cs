using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [field: SerializeField] public MainButtons MainButtons {get; private set;}
        [field: SerializeField] public LevelSelector LevelSelector {get; private set;}

        private void Awake() 
        {
            ShowMainButtons();
        }

        private void OnEnable() 
        {
            MainButtons.PlayClickedEvent.AddListener(ShowLevelSelector);
            LevelSelector.BackPressed.AddListener(ShowMainButtons);
        }

        private void OnDisable() 
        {
            MainButtons.PlayClickedEvent.RemoveListener(ShowLevelSelector);
            LevelSelector.BackPressed.RemoveListener(ShowMainButtons);
        }

        private void ShowMainButtons()
        {
            MainButtons.gameObject.SetActive(true);
            LevelSelector.gameObject.SetActive(false);
        }

        private void ShowLevelSelector()
        {
            MainButtons.gameObject.SetActive(false);
            LevelSelector.gameObject.SetActive(true);
        }

    }
}

