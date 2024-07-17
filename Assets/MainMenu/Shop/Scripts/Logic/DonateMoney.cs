using Common.Data;
using DI;
using UnityEngine;

namespace MainMenu.Shop.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new donate money", fileName = "Donate money")]
    public class DonateMoney : ShopItem
    {
        [field: SerializeField, Min(0)] public int Cost {get; private set;}
        [field: SerializeField, Min(0)] public int CoinsReward {get; private set;}

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
