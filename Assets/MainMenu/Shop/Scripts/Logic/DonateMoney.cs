using Common.Data;
using Common.Disposables;
using DI;
using UnityEngine;
using YG;

namespace MainMenu.Shop.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new donate money", fileName = "Donate money")]
    public class DonateMoney : ShopItem
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField, Min(0)] public int Cost { get; private set; }
        [field: SerializeField, Min(0)] public int CoinsReward { get; private set; }

        public override void Init(DIContainer sceneContext)
        {
            void onPurchaseSuccessEvent(string id)
            {
                if (id == Id)
                {
                    sceneContext.Get<IPlayerData>().AddCoins(CoinsReward);
                    Claim();
                    YandexGame.SaveProgress();
                }
            }

            YandexGame.PurchaseSuccessEvent += onPurchaseSuccessEvent;

            sceneContext.Get<CompositeDisposable>(MainMenu.Bootstrap.MAIN_MENU_DISPOSABLES_TAG)
                .Add(new DisposableDelegate(() => YandexGame.PurchaseSuccessEvent -= onPurchaseSuccessEvent));
        }

        public override bool TryClaim(DIContainer sceneContext)
        {
            YandexGame.BuyPayments(Id);
            return true;
        }
    }
}