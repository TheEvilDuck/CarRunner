using UnityEngine;

namespace Services.PurchaseService.FakeStorePurchases
{
    [CreateAssetMenu(fileName = "FakeStorePurchase", menuName = "FakeStore/FakeStorePurchaseData")]
    public class FakeStoreData: ScriptableObject, IPurchaseData
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public string Price { get; private set;}
        [field: SerializeField] public Sprite CurrencyIcon { get; private set;}
        [field: SerializeField] public Sprite ItemIcon { get; private set;}
    }
}