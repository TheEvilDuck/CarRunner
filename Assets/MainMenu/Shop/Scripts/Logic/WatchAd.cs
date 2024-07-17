using System;
using Common.Data;
using DI;
using UnityEngine;

namespace MainMenu.Shop.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new watch ad", fileName = "Watch ad")]
    public class WatchAd : ShopItem
    {
        [field: SerializeField, Min(0)] public int CoinsReward {get; private set;}
        [field: SerializeField, Min(0)] public double AdCooldown {get; private set;}

        public Func<float> GetCurrentCooldown {get; private set;} = () => 0;

        public override bool TryClaim(DIContainer sceneContext)
        {
            IPlayerData playerData = sceneContext.Get<IPlayerData>();

            if ((DateTime.Now - playerData.WatchShopAdLastTime).TotalSeconds >= AdCooldown)
            {
                playerData.SaveWatchAdLastTime();
                //TODO change it to invoke SDK ad, OnWatchedAd must be subscriber of ad watched event
                OnAdWatched(sceneContext);
                return true;
            }

            return false;
        
        }

        public override void Init(DIContainer sceneContext)
        {
            GetCurrentCooldown = () =>
            {
                return Mathf.Max(0, (float)(AdCooldown - (DateTime.Now - sceneContext.Get<IPlayerData>().WatchShopAdLastTime).TotalSeconds));
            };
        }

        private void OnAdWatched(DIContainer sceneContext)
        {
            sceneContext.Get<IPlayerData>().AddCoins(CoinsReward);
            Claim();
        }
    }
}
