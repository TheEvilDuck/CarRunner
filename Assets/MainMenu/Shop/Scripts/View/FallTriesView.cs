using System;
using Infrastructure.DI;
using MainMenu.Shop.Scripts.Logic;
using TMPro;
using UnityEngine;

namespace MainMenu.Shop.Scripts.View
{
    public class FallTriesView : ShopItemView
    {
        [SerializeField] private TextMeshProUGUI _cost;
        private FallTries _fallTries;
        private IDIContainer _container;

        public override void Init(ShopItem shopItem, IDIContainer container)
        {
            if (shopItem is not FallTries fallTries)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");

            _fallTries = fallTries;
            _container = container;
            UpdateCost();

            _fallTries.claimed += UpdateCost;
        }

        private void OnDestroy() => _fallTries.claimed -= UpdateCost;

        private void UpdateCost() => _cost.text = _fallTries.GetFinalCost(_container).ToString();
    }
}