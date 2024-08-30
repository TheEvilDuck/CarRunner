using System;
using Common;
using Common.Data;
using DI;
using UnityEngine;
using YG;

namespace MainMenu.Shop.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new watch ad", fileName = "Watch ad")]
    public class WatchAd : ShopItem
    {
        public const int WATCH_AD_SHOP_ID = 1;
        [field: SerializeField, Min(0)] public int CoinsReward {get; private set;}
        [field: SerializeField, Min(0)] public double AdCooldown {get; private set;}

        public Func<float> GetCurrentCooldown {get; private set;} = () => 0;

        public override bool TryClaim(DIContainer sceneContext)
        {
            IPlayerData playerData = sceneContext.Get<IPlayerData>();

            if ((DateTime.Now - playerData.WatchShopAdLastTime).TotalSeconds >= AdCooldown)
            {
                void OnRewardVideoEvent(int id)
                {
                    if (id != WATCH_AD_SHOP_ID)
                        return;

                    playerData.SaveWatchAdLastTime();
                    sceneContext.Get<PauseManager>().Resume();

                    YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
                    OnAdWatched(sceneContext);
                }

                YandexGame.RewardVideoEvent += OnRewardVideoEvent;
                sceneContext.Get<PauseManager>().Pause();
                YandexGame.RewVideoShow(WATCH_AD_SHOP_ID);
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
