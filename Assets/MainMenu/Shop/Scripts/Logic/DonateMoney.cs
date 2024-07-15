using UnityEngine;

namespace MainMenu.Shop.Logic
{
    public class DonateMoney : ShopItem
    {
        [field: SerializeField, Min(0)] public int Cost {get; private set;}
        [field: SerializeField, Min(0)] public int CoinsReward {get; private set;}
    }
}
