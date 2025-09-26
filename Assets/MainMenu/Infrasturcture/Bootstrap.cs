using Common.Disposables;
using Infrastructure.Bootstraps;
using Infrastructure.DI;
using MainMenu.LanguageSelection.Scripts;
using MainMenu.Shop.Scripts;
using Services.Localization.Scripts;
using Services.PlayerData;
using UnityEngine;

namespace MainMenu.Infrasturcture
{
    public class Bootstrap : MonoBehaviourBootstrap
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private NotEnoughMoneyPopup _notEnoughMoneyPopup;
        [SerializeField] private CoinsView _coinsView;
        [SerializeField] private ShopItemFactory _shopItemFactory;
        
        public override void MakeRegistrationsInto(DIContainer container)
        {
            container.Register(() => new CompositeDisposable(), MainMenuTags.DISPOSABLES);
            container.Register(_mainMenuView);
            container.Register(_mainMenuView.SettingsMenu);
            container.Register(_mainMenuView.LevelSelector);
            container.Register(_mainMenuView.ShopView);
            container.Register(_mainMenuView.TutorialView);
            container.Register(_mainMenuView.LanguageSelectorMenu);
            container.Register(_notEnoughMoneyPopup);
            container.Register(_coinsView);
            container.Register(_shopItemFactory);
        }

        protected override IBootstrapInitializer GetInnerInitializer(IDIContainer container)
            => new Initializer(_mainMenuView, _shopItemFactory);
    }
}