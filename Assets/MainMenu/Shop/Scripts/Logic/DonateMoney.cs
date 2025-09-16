using System;
using System.Collections;
using System.Linq;
using Infrastructure.DI;
using Services.PlayerData;
using Services.PurchaseService;
using UnityEngine;

namespace MainMenu.Shop.Scripts.Logic
{
    [CreateAssetMenu(menuName = "Shop/Shop items/new donate money", fileName = "Donate money")]
    public class DonateMoney : ShopItem
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int CurrencyAmount { get; private set; }

        public override bool CanBeClaimed(IDIContainer container)
        {
            IPurchaseService purchaseService = container.Get<IPurchaseService>();
            return purchaseService.Purchases.FirstOrDefault(data => data.ID == Id) != default;
        }

        protected override IEnumerator Claim(IDIContainer container, Action<bool> callback)
        {
            IPurchaseService purchaseService = container.Get<IPurchaseService>();
            
            void OnPurchaseProceed(bool success)
            {
                if (success)
                    container.Get<IPlayerData>().AddCoins(CurrencyAmount);
                
                callback?.Invoke(success);
            }

            yield return purchaseService.HandlePurchase(Id, OnPurchaseProceed);
        }
    }
}