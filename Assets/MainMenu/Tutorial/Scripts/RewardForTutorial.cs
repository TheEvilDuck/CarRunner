using Common.Data;
using DI;
using MainMenu.Shop.Logic;
using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(fileName = "TutorialReward")]
    public class RewardForTutorial : ShopItem
    {
        [field: SerializeField, Min(0)] public int CoinsReward { get; private set; }

        public override bool TryClaim(DIContainer sceneContext)
        {
            //TODO replace this true to SDK check for yandex coins
            if (true)
            {
                sceneContext.Get<IPlayerData>().AddCoins(CoinsReward);
                Claim();
                return true;
            }

            return false;
        }
    }
}