using System;
using System.Collections;

namespace Services.Ads
{
    public interface IAdsService
    {
        public event Action adIsShown;
        public event Action adShowingStarted;
        public event Action<int> rewardAdIsShown;
        
        public bool Ready { get; }
        public IEnumerator ShowFullscreenAd();
        public IEnumerator ShowRewardedAd(int adId, Action<bool> callback);
    }
}