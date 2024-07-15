using UnityEngine;

namespace MainMenu.Shop.Logic
{
    public class WatchAd : ShopItem
    {
        [field: SerializeField, Min(0)] public int CoinsReward {get; private set;}
        [field: SerializeField, Min(0)] public float AdCooldown {get; private set;}
    }
}
