using Common.MenuParent;
using MainMenu.LevelSelection;
using MainMenu.Shop.View;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [field: SerializeField] public MainButtons MainButtons {get; private set;}
        [field: SerializeField] public LevelSelector LevelSelector {get; private set;}
        [field: SerializeField] public SettingsMenu SettingsMenu { get; private set;}
        [field: SerializeField] public ShopView ShopView {get; private set;}

        private MenuParentsManager _menuParentsManager;

        private void Awake() 
        {
            _menuParentsManager = new MenuParentsManager();
            _menuParentsManager.Add(MainButtons);
            _menuParentsManager.Add(LevelSelector);
            _menuParentsManager.Add(SettingsMenu);
            _menuParentsManager.Add(ShopView);
            ShowMainButtons();
        }

        private void OnEnable() 
        {
            MainButtons.PlayClickedEvent.AddListener(ShowLevelSelector);
            MainButtons.SettingsClickedEvent.AddListener(ShowSettingsMenu);
            MainButtons.ShopClicked.AddListener(ShowShopMenu);
            LevelSelector.BackPressed.AddListener(ShowMainButtons);
            SettingsMenu.BackPressed.AddListener(ShowMainButtons);
            ShopView.BackPressed.AddListener(ShowMainButtons);
        }

        private void OnDisable() 
        {
            MainButtons.PlayClickedEvent.RemoveListener(ShowLevelSelector);
            MainButtons.SettingsClickedEvent.RemoveListener(ShowSettingsMenu);
            MainButtons.ShopClicked.RemoveListener(ShowShopMenu);
            LevelSelector.BackPressed.RemoveListener(ShowMainButtons);
            SettingsMenu.BackPressed.RemoveListener(ShowMainButtons);
            ShopView.BackPressed.RemoveListener(ShowMainButtons);
        }

        private void ShowMainButtons() => _menuParentsManager.Show(MainButtons);

        private void ShowLevelSelector() => _menuParentsManager.Show(LevelSelector);

        private void ShowSettingsMenu() => _menuParentsManager.Show(SettingsMenu);
        public void ShowShopMenu() => _menuParentsManager.Show(ShopView);
    }
}

