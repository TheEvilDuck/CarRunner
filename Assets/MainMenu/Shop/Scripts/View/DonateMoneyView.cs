using System;
using System.Linq;
using Infrastructure.DI;
using MainMenu.Shop.Scripts.Logic;
using Services.PurchaseService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Shop.Scripts.View
{
    public class DonateMoneyView : ShopItemView
    {
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Image _costImage;
        [SerializeField] private Image _itemImage;
        
        public override void Init(ShopItem shopItem, IDIContainer container)
        {
            if (shopItem is not DonateMoney donateMoney)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");
            
            IPurchaseService purchaseService = container.Get<IPurchaseService>();
            IPurchaseData purchase = purchaseService.Purchases.FirstOrDefault(x => x.ID == donateMoney.Id);

            if (purchase != null)
            {
                _costText.text = purchase.Price;
                _rewardText.text = donateMoney.CurrencyAmount.ToString();
                SetCurrencyImage(purchase.CurrencyIcon);
                SetItemImage(purchase.ItemIcon);
            }
        }

        private void SetCurrencyImage(Sprite sprite)
        {
            _costImage.sprite = sprite;
        }

        private void SetItemImage(Sprite sprite)
        {
            _itemImage.sprite = sprite;
        }
    }
}
