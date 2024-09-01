using System;
using MainMenu.Shop.Logic;
using Services.Localization;
using TMPro;
using UnityEngine;

namespace MainMenu.Shop.View
{
    public class FallTriesView : ShopItemView
    {
        private const string LOCALIZATION_ID = "fallTries";
        [SerializeField] private TextMeshProUGUI _cost;
        private FallTries _fallTries;
        public string TextId => LOCALIZATION_ID;
        public event Action<ILocalizable> updateRequested;

        public override void Init(ShopItem shopItem)
        {
            if (shopItem is not FallTries fallTries)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");

            _cost.text = fallTries.FinalCost().ToString();
            _fallTries = fallTries;

            _fallTries.claimed += UpdateCost;
        }

        private void OnDestroy() => _fallTries.claimed -= UpdateCost;

        private void UpdateCost() => _cost.text = _fallTries.FinalCost().ToString();
    }
}