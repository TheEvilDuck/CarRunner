using Common.Data;
using DI;
using UnityEngine;

namespace MainMenu.Shop.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new FallTries", fileName = "FallTries")]
    public class FallTries : ShopItem
    {
        [SerializeField, Min(1)] private int _costCoefficient;
        private IPlayerData _playerData;
        [field: SerializeField, Min(0)] public int BaseCost { get; private set; }

        public override void Init(DIContainer sceneContext) => _playerData = sceneContext.Get<IPlayerData>();

        public override bool TryClaim(DIContainer sceneContext)
        {
            IPlayerData playerData = sceneContext.Get<IPlayerData>();

            if (playerData.Coins < FinalCost())
                return false;

            playerData.SpendCoins(FinalCost());
            playerData.AddOrSubtractFallTries(1);
            Claim();
            return true;
        }

        public int FinalCost() => BaseCost * (1 + (_playerData.MaxFallTries - 1) * _costCoefficient);
    }
}