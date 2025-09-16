using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.PurchaseService.FakeStorePurchases
{
    public class FakeStorePurchaseService: IPurchaseService
    {
        private readonly List<FakeStoreData> _fakeStoreDatas = new List<FakeStoreData>();
        
        public IReadOnlyList<IPurchaseData> Purchases => _fakeStoreDatas;

        public IEnumerator HandlePurchase(string id, Action<bool> callback)
        {
            FakeStoreData data = _fakeStoreDatas.FirstOrDefault(d => d.ID == id);

            if (data == null)
            {
                callback?.Invoke(false);
                yield break;
            }
            
            callback?.Invoke(true);
        }

        public IEnumerator Initialize()
        {
            var req = Resources.LoadAll<FakeStoreData>("");
            _fakeStoreDatas.AddRange(req);
            yield break;
        }
    }
}