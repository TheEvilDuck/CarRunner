using Common.MenuParent;
using Common.UI;
using MainMenu.LanguageSelection;
using MainMenu.LevelSelection;
using MainMenu.Shop.View;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        private MenuParentsManager _menuParentsManager;

        [field: SerializeField] public MainButtons MainButtons {get; private set;}
        [field: SerializeField] public LevelSelector LevelSelector {get; private set;}
        [field: SerializeField] public GameSettingsUI SettingsMenu { get; private set;}
        [field: SerializeField] public ShopView ShopView {get; private set;}
        [field: SerializeField] public TutorialView TutorialView { get; private set;}
        [field: SerializeField] public LanguageSelectorMenu LanguageSelectorMenu {get; private set;}
        [field: SerializeField] public CreditsView CreditsMenu {get; private set;}

        public void Init()
        {
            _menuParentsManager = new MenuParentsManager();
            _menuParentsManager.Add(MainButtons);
            _menuParentsManager.Add(LevelSelector);
            _menuParentsManager.Add(SettingsMenu);
            _menuParentsManager.Add(ShopView);
            _menuParentsManager.Add(TutorialView);
            _menuParentsManager.Add(LanguageSelectorMenu);
            _menuParentsManager.Add(CreditsMenu);
            ShowMainButtons();
        }

        private void OnEnable() 
        {
            MainButtons.PlayClickedEvent.AddListener(ShowLevelSelector);
            MainButtons.SettingsClickedEvent.AddListener(ShowSettingsMenu);
            MainButtons.ShopClicked.AddListener(ShowShopMenu);
            MainButtons.TutorialClicked.AddListener(ShowTutorial);
            MainButtons.CreditsClicked.AddListener(ShowCredits);
            LevelSelector.BackPressed.AddListener(ShowMainButtons);
            SettingsMenu.BackPressed.AddListener(ShowMainButtons);
            ShopView.BackPressed.AddListener(ShowMainButtons);
            TutorialView.BackPressed.AddListener(ShowMainButtons);
            TutorialView.UnderstandablePressed.AddListener(ShowMainButtons);
            MainButtons.LanguageClicked.AddListener(ShowLanguageMenu);
            LanguageSelectorMenu.BackPressed.AddListener(ShowMainButtons);
            CreditsMenu.BackPressed.AddListener(ShowMainButtons);
        }

        private void OnDisable() 
        {
            MainButtons.PlayClickedEvent.RemoveListener(ShowLevelSelector);
            MainButtons.SettingsClickedEvent.RemoveListener(ShowSettingsMenu);
            MainButtons.ShopClicked.RemoveListener(ShowShopMenu);
            MainButtons.TutorialClicked.RemoveListener(ShowTutorial);
            MainButtons.CreditsClicked.RemoveListener(ShowCredits);
            LevelSelector.BackPressed.RemoveListener(ShowMainButtons);
            SettingsMenu.BackPressed.RemoveListener(ShowMainButtons);
            ShopView.BackPressed.RemoveListener(ShowMainButtons);
            TutorialView.BackPressed.RemoveListener(ShowMainButtons);
            TutorialView.UnderstandablePressed.RemoveListener(ShowMainButtons);
            MainButtons.LanguageClicked.RemoveListener(ShowLanguageMenu);
            LanguageSelectorMenu.BackPressed.RemoveListener(ShowMainButtons);
            CreditsMenu.BackPressed.RemoveListener(ShowMainButtons);
        }

        private void ShowMainButtons() => _menuParentsManager.Show(MainButtons);

        private void ShowLevelSelector() => _menuParentsManager.Show(LevelSelector);

        private void ShowSettingsMenu() => _menuParentsManager.Show(SettingsMenu);

        private void ShowTutorial() => _menuParentsManager.Show(TutorialView); 

        private void ShowLanguageMenu() => _menuParentsManager.Show(LanguageSelectorMenu);

        public void ShowShopMenu() => _menuParentsManager.Show(ShopView);

        public void ShowCredits() => _menuParentsManager.Show(CreditsMenu);
    }
}

