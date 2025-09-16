using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.CoroutinePerformer;
using Infrastructure.DI;
using MainMenu.Shop.Scripts.Logic;
using MainMenu.Shop.Scripts.View;
using Services.PurchaseService;
using Services.SpriteURLLoading;
using UnityEngine;

namespace MainMenu.Shop.Scripts
{
    [CreateAssetMenu(menuName = "Shop/New shop factory", fileName = "Shop item factory")]
    public class ShopItemFactory : ScriptableObject
    {
        [SerializeField] private List<ShopItemAndPrefab> _shopContent;

        public IEnumerable<ShopItemView> GetSetUpView(Transform parent, IDIContainer container)
        {
            List<ShopItemView> result = new List<ShopItemView>();

            foreach (ShopItemAndPrefab shopItemAndPrefab in _shopContent)
            {
                ShopItemView shopItemView = Instantiate(shopItemAndPrefab.ShopItemViewPrefab, parent);
                shopItemView.Init(shopItemAndPrefab.ShopItem, container);
                
                ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();

                void OnProceed(bool success)
                {
                    // тут можно мб ошибку обработать или еще что
                }

                IEnumerator StartPurchase() => shopItemAndPrefab.ShopItem.TryClaim(container, OnProceed);

                shopItemView.Clicked.AddListener(() => coroutinePerformer.StartCoroutine(StartPurchase()));
            }

            return result;
        }

        [Serializable]
        private class ShopItemAndPrefab
        {
            [field: SerializeField] public ShopItem ShopItem;
            [field: SerializeField] public ShopItemView ShopItemViewPrefab;
        }
    }
}
