using System;
using System.Collections.Generic;
using DI;
using MainMenu.Shop.Logic;
using MainMenu.Shop.View;
using UnityEngine;

namespace MainMenu.Shop
{
    [CreateAssetMenu(menuName = "Shop/New shop factory", fileName = "Shop item factory")]
    public class ShopItemFactory : ScriptableObject
    {
        [SerializeField] private List<ShopItemAndPrefab> _shopContent;

        public IEnumerable<ShopItemView> GetSetUpView(Transform parent, DIContainer sceneContext)
        {
            List<ShopItemView> result = new List<ShopItemView>();

            foreach (ShopItemAndPrefab shopItemAndPrefab in _shopContent)
            {
                ShopItemView shopItemView = Instantiate(shopItemAndPrefab.ShopItemViewPrefab, parent);
                shopItemAndPrefab.ShopItem.Init(sceneContext);
                shopItemView.Init(shopItemAndPrefab.ShopItem);

                shopItemView.Clicked.AddListener(() => shopItemAndPrefab.ShopItem.TryClaim(sceneContext));
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
