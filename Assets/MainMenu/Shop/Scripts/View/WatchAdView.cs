using System;
using System.Collections;
using MainMenu.Shop.Logic;
using TMPro;
using UnityEngine;

namespace MainMenu.Shop.View
{
    public class WatchAdView : ShopItemView
    {
        [SerializeField] private TextMeshProUGUI _rewardText;
        private Coroutine _currentTimer;
        private WatchAd _watchAd;
        public override void Init(ShopItem shopItem)
        {
            if (shopItem is not WatchAd watchAd)
                throw new ArgumentException($"Somehow you passed wrong shopitem to view, you passed {shopItem.name}");

            _rewardText.text = watchAd.CoinsReward.ToString();
            _watchAd = watchAd;
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
                _rewardText.text = _watchAd.CoinsReward.ToString(); 
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
                _rewardText.text = Mathf.CeilToInt((float)_watchAd.GetCurrentCooldown.Invoke()).ToString();
                yield return null;
            }

            _rewardText.text = _watchAd.CoinsReward.ToString(); 

            _currentTimer = null;
        }
    }
}
