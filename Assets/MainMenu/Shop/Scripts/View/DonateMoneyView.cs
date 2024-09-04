using System;
using MainMenu.Shop.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Shop.View
{
    public class DonateMoneyView : ShopItemView
    {
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Image _costImage;
        public override void Init(ShopItem shopItem)
        {
            if (shopItem is not DonateMoney donateMoney)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");

            _costText.text = donateMoney.Cost.ToString();
            _rewardText.text = donateMoney.CoinsReward.ToString();
        }

        public override void SetCurrencyImage(Sprite sprite)
        {
            _costImage.sprite = sprite;
        }
    }
}
