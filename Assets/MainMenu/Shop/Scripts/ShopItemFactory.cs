using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using DI;
using MainMenu.Shop.Logic;
using MainMenu.Shop.View;
using Services.SpriteURLLoading;
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
                        SpriteURLLoader currencyImageLoader = new SpriteURLLoader();
                        SpriteURLLoader itemImageLoader = new SpriteURLLoader();

                        currencyImageLoader.LoadedSprite.changed += OnCurrencyImageLoaded;
                        itemImageLoader.LoadedSprite.changed += OnItemImageLoaded;

                        var coroutines = sceneContext.Get<Coroutines>();

                        void OnCurrencyImageLoaded(Sprite sprite)
                        {
                            shopItemView.SetCurrencyImage(sprite);
                            currencyImageLoader.LoadedSprite.changed -= OnCurrencyImageLoaded;
                        }

                        void OnItemImageLoaded(Sprite sprite)
                        {
                            shopItemView.SetItemImage(sprite);
                            itemImageLoader.LoadedSprite.changed -= OnItemImageLoaded;
                        }

                        coroutines.StartCoroutine(currencyImageLoader.Load(purchase.currencyImageURL));
                        coroutines.StartCoroutine(itemImageLoader.Load(purchase.imageURI));
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
