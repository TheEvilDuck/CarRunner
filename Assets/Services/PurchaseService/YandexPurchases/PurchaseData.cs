using System;
using System.Collections;
using Common.CoroutinePerformer;
using Services.SpriteURLLoading;
using UnityEngine;

namespace Services.PurchaseService.YandexPurchases
{
    public class PurchaseData: IPurchaseData
    {
        private readonly string _currencyImageURL;
        private readonly string _imageURL;

        public bool Loaded { get; private set; } = false;

        public PurchaseData(
            string id,
            string price,
            string currencyImageURL, 
            string imageURL)
        {
            _currencyImageURL = currencyImageURL;
            _imageURL = imageURL;
            ID = id;
            Price = price;
        }

        public string ID { get; }
        public string Price { get; }
        public Sprite CurrencyIcon { get; private set;}
        public Sprite ItemIcon { get; private set;}

        public IEnumerator Load()
        {
            SpriteURLLoader currencyImageLoader = new SpriteURLLoader();
            SpriteURLLoader itemImageLoader = new SpriteURLLoader();

            currencyImageLoader.LoadedSprite.changed += OnCurrencyImageLoaded;
            itemImageLoader.LoadedSprite.changed += OnItemImageLoaded;
            currencyImageLoader.error += OnCurrencyImageError;
            itemImageLoader.error += OnItemImageError;

            bool error = false;

            void OnCurrencyImageLoaded(Sprite sprite)
            {
                currencyImageLoader.LoadedSprite.changed -= OnCurrencyImageLoaded;
                currencyImageLoader.error -= OnCurrencyImageError;
                itemImageLoader.error -= OnItemImageError;
            }

            void OnItemImageLoaded(Sprite sprite)
            {
                itemImageLoader.LoadedSprite.changed -= OnItemImageLoaded;
            }

            void OnCurrencyImageError()
            {
                currencyImageLoader.LoadedSprite.changed -= OnCurrencyImageLoaded;
                currencyImageLoader.error -= OnCurrencyImageError;
                error = false;
            }

            void OnItemImageError()
            {
                itemImageLoader.LoadedSprite.changed -= OnItemImageLoaded;
                itemImageLoader.error -= OnItemImageError;
                error = false;
            }

            yield return currencyImageLoader.Load(_currencyImageURL);
            yield return itemImageLoader.Load(_imageURL);

            if (error == false)
                Loaded = true;
        }
    }
}