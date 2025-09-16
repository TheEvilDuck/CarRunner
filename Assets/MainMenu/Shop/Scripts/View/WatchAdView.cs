using System;
using System.Collections;
using Infrastructure.DI;
using MainMenu.Shop.Scripts.Logic;
using Services.Localization.Scripts;
using TMPro;
using UnityEngine;

namespace MainMenu.Shop.Scripts.View
{
    public class WatchAdView : ShopItemView, ILocalizable
    {
        private const string LOCALIZATION_ID = "watch_ad";
        
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _watchAdText;
        
        private Coroutine _currentTimer;
        private WatchAd _watchAd;
        private IDIContainer _container;

        public event Action<ILocalizable> updateRequested;

        public string TextId => LOCALIZATION_ID;

        public override void Init(ShopItem shopItem, IDIContainer container)
        {
            if (shopItem is not WatchAd watchAd)
                throw new ArgumentException(
                    $"You must pass to watch ad view only watch ad shop items! {shopItem.GetType()} passed!");
            
            _rewardText.text = watchAd.CoinsReward.ToString();
            _watchAd = watchAd;
            _container = container;

            LocalizationRegistrator.Instance.RegisterLocalizable(this, true);
        }

        private void OnEnable() 
        {
            _watchAd.claimed += OnRewardClaimed;

            if (_watchAd.GetCurrentCooldown(_container) > 0)
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
            while (_watchAd.GetCurrentCooldown(_container) > 0)
            {
                _watchAdText.text = Mathf.CeilToInt(_watchAd.GetCurrentCooldown(_container)).ToString();
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
