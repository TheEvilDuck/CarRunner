using UnityEngine;

namespace Services.PurchaseService
{
    public interface IPurchaseData
    {
        public string ID { get; }
        public string Price { get; }
        public Sprite CurrencyIcon { get; }
        public Sprite ItemIcon { get; }
    }
}