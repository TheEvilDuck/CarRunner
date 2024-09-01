using Common.Data;
using DI;
using UnityEngine;

namespace MainMenu.Shop.Logic
{
    public class FallTries : ShopItem
    {
        [SerializeField, Min(1)] private int _costCoefficient;
        [SerializeField, Min(0)] private int _baseCost;

        public override bool TryClaim(DIContainer sceneContext)
        {
            IPlayerData playerData = sceneContext.Get<IPlayerData>();

            if (playerData.Coins < FinalCost(_baseCost))
                return false;

            playerData.SpendCoins(FinalCost(_baseCost));
            playerData.AddOrSubtractFallTries(1);
            return true;
        }

        private int FinalCost(int MaxFallTries)
        {
            return _baseCost * (1 + (MaxFallTries - 1) * _costCoefficient);
        }
    }
}