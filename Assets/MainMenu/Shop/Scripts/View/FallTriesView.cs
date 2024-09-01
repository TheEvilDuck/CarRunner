using System;
using MainMenu.Shop.Logic;
using TMPro;
using UnityEngine;

namespace MainMenu.Shop.View
{
    public class FallTriesView : ShopItemView
    {
        [SerializeField] private TextMeshProUGUI _cost;
        private FallTries _fallTries;

        public override void Init(ShopItem shopItem)
        {
            if (shopItem is not FallTries fallTries)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");

            _fallTries = fallTries;
            UpdateCost();

            _fallTries.claimed += UpdateCost;
        }

        private void OnDestroy() => _fallTries.claimed -= UpdateCost;

        private void UpdateCost() => _cost.text = _fallTries.FinalCost().ToString();
    }
}