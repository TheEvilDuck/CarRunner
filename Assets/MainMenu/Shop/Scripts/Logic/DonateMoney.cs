using System;
using System.Collections.Generic;
using Common.Data;
using DI;
using UnityEngine;
using YG;

namespace MainMenu.Shop.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new donate money", fileName = "Donate money")]
    public class DonateMoney : ShopItem, IDisposable
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField, Min(0)] public int Cost { get; private set; }
        [field: SerializeField, Min(0)] public int CoinsReward { get; private set; }
        private Action<string> _onPurchaseSuccessEvent;

        public void Dispose()
        {
            YandexGame.PurchaseSuccessEvent -= _onPurchaseSuccessEvent;
        }

        public override void Init(DIContainer sceneContext)
        {
            sceneContext.Get<List<IDisposable>>().Add(this);

            void onPurchaseSuccessEvent(string id)
            {
                if (id == Id)
                {
                    sceneContext.Get<IPlayerData>().AddCoins(CoinsReward);
                    Claim();
                }
            }

            _onPurchaseSuccessEvent = onPurchaseSuccessEvent;

            YandexGame.PurchaseSuccessEvent += _onPurchaseSuccessEvent;
        }

        public override bool TryClaim(DIContainer sceneContext)
        {
            YandexGame.BuyPayments(Id);
            return true;
        }
    }
}