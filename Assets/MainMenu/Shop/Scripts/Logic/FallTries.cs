using System;
using System.Collections;
using Infrastructure.DI;
using Services.PlayerData;
using UnityEngine;

namespace MainMenu.Shop.Scripts.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new FallTries", fileName = "FallTries")]
    public class FallTries : ShopItem
    {
        [SerializeField, Min(0)] private int _baseCost;
        [SerializeField, Min(1)] private int _costCoefficient;

        public int GetFinalCost(IDIContainer container) 
            => _baseCost * (1 + (container.Get<IPlayerData>().MaxFallTries - 1) * _costCoefficient);
        
        public override bool CanBeClaimed(IDIContainer container)
        {
            IPlayerData playerData = container.Get<IPlayerData>();
            return playerData.Coins >= GetFinalCost(container);
        }

        protected override IEnumerator Claim(IDIContainer container, Action<bool> callback)
        {
            IPlayerData playerData = container.Get<IPlayerData>();

            int cost = GetFinalCost(container);
            
            if (cost > 0)
                playerData.SpendCoins(cost);
            
            playerData.AddOrSubtractFallTries(1);
            callback?.Invoke(true);
            yield break;
        }
    }
}