using System;
using System.Collections;
using MainMenu.Shop.Logic;
using Services.Localization;
using TMPro;
using UnityEngine;

namespace MainMenu.Shop.View
{
    public class WatchAdView : ShopItemView, ILocalizable
    {
        private const string LOCALIZATION_ID = "watch_ad";
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _watchAdText;
        private Coroutine _currentTimer;
        private WatchAd _watchAd;

        public event Action<ILocalizable> updateRequested;

        public string TextId => LOCALIZATION_ID;

        public override void Init(ShopItem shopItem)
        {
            if (shopItem is not WatchAd watchAd)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");

            _rewardText.text = watchAd.CoinsReward.ToString();
            _watchAd = watchAd;

            LocalizationRegistrator.Instance.RegisterLocalizable(this, true);
        }

        private void OnEnable() 
        {
            _watchAd.claimed += OnRewardClaimed;

            if ((float)_watchAd.GetCurrentCooldown.Invoke() > 0)
            {
                if (_currentTimer != null)
                    StopCoroutine(_currentTimer);

                _currentTimer = StartCoroutine(CDTimer());
            }
            else
            {
                updateRequested?.Invoke(this);
            }
        }

        private void OnDisable() 
        {
            _watchAd.claimed -= OnRewardClaimed;

            if (_currentTimer != null)
                    StopCoroutine(_currentTimer);
        }

        private void OnRewardClaimed()
        {
            _currentTimer = StartCoroutine(CDTimer());
        }

        private IEnumerator CDTimer()
        {
            while ((float)_watchAd.GetCurrentCooldown.Invoke() > 0)
            {
                _watchAdText.text = Mathf.CeilToInt((float)_watchAd.GetCurrentCooldown.Invoke()).ToString();
                yield return null;
            }

            updateRequested?.Invoke(this);
            _currentTimer = null;
        }

        public void UpdateText(string text)
        {
            _watchAdText.text = text;
        }
    }
}
