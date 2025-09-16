using System;
using System.Collections;
using Infrastructure.DI;
using Services.Ads;
using Services.PlayerData;
using UnityEngine;

namespace MainMenu.Shop.Scripts.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new watch ad", fileName = "Watch ad")]
    public class WatchAd : ShopItem
    {
        public const int WATCH_AD_SHOP_ID = 1;
        [field: SerializeField, Min(0)] public int CoinsReward {get; private set;}
        [field: SerializeField, Min(0)] public double AdCooldown {get; private set;}

        public float GetCurrentCooldown(IDIContainer container)
        {
            return Mathf.Max(0,
                (float)(AdCooldown - (DateTime.Now - container.Get<IPlayerData>().WatchShopAdLastTime).TotalSeconds));
        }
        
        public override bool CanBeClaimed(IDIContainer container)
        {
            if (GetCurrentCooldown(container) < AdCooldown)
                return false;

            IAdsService adsService = container.Get<IAdsService>();
            return adsService.Ready;
        }

        protected override IEnumerator Claim(IDIContainer container, Action<bool> callback)
        {
            IAdsService adsService = container.Get<IAdsService>();

            void OnAdShown(bool fullyShown)
            {
                if (fullyShown)
                    container.Get<IPlayerData>().AddCoins(CoinsReward);
                
                callback?.Invoke(fullyShown);
            }

            yield return adsService.ShowRewardedAd(WATCH_AD_SHOP_ID, OnAdShown);
        }
    }
}
