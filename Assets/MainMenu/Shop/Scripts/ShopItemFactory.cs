using System;
using System.Collections.Generic;
using System.Linq;
using DI;
using MainMenu.Shop.Logic;
using MainMenu.Shop.View;
using UnityEngine;
using YG;
using YG.Utils.Pay;

namespace MainMenu.Shop
{
    [CreateAssetMenu(menuName = "Shop/New shop factory", fileName = "Shop item factory")]
    public class ShopItemFactory : ScriptableObject
    {
        [SerializeField] private List<ShopItemAndPrefab> _shopContent;

        public IEnumerable<ShopItemView> GetSetUpView(Transform parent, DIContainer sceneContext)
        {
            List<ShopItemView> result = new List<ShopItemView>();
            var purchases = sceneContext.Get<Purchase[]>().ToList();
            var currencyImageLoad = sceneContext.Get<ImageLoadYG>();

            foreach (ShopItemAndPrefab shopItemAndPrefab in _shopContent)
            {
                ShopItemView shopItemView = Instantiate(shopItemAndPrefab.ShopItemViewPrefab, parent);
                shopItemAndPrefab.ShopItem.Init(sceneContext);
                shopItemView.Init(shopItemAndPrefab.ShopItem);

                if (shopItemAndPrefab.ShopItem is DonateMoney donateMoney)
                {
                    var purchase = purchases.Find(x => x.id == donateMoney.Id);

                    if (purchase != null)
                    {
                        currencyImageLoad.Load(purchase.currencyImageURL);
                        shopItemView.SetCurrencyImage(currencyImageLoad.spriteImage.sprite);
                    }
                }

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
