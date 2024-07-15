using System;
using MainMenu.Shop.Logic;
using TMPro;
using UnityEngine;

namespace MainMenu.Shop.View
{
    public class WatchAdView : ShopItemView
    {
        [SerializeField] private TextMeshProUGUI _rewardText;
        public override void Init(ShopItem shopItem)
        {
            if (shopItem is not WatchAd watchAd)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");

            _rewardText.text = watchAd.CoinsReward.ToString();
        }
    }
}
