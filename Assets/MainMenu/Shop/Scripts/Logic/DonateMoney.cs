using Common.Data;
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
                YandexGame.PurchaseSuccessEvent -= onPurchaseSuccessEvent;

                if (id == Id)
                {
                    sceneContext.Get<IPlayerData>().AddCoins(CoinsReward);
                    Claim();
                }
            }

            YandexGame.PurchaseSuccessEvent += onPurchaseSuccessEvent;
            YandexGame.ConsumePurchases();
        }

        public override bool TryClaim(DIContainer sceneContext)
        {
            void onPurchaseSuccessEvent(string id)
            {
                YandexGame.PurchaseSuccessEvent -= onPurchaseSuccessEvent;

                if (id == Id)
                {
                    sceneContext.Get<IPlayerData>().AddCoins(CoinsReward);
                    Claim();
                }
            }

            YandexGame.PurchaseSuccessEvent += onPurchaseSuccessEvent;
            YandexGame.BuyPayments(Id);
            return true;
        }
    }
}