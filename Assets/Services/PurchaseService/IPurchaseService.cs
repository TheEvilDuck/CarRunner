using System;
using System.Collections;
using System.Collections.Generic;

namespace Services.PurchaseService
{
    public interface IPurchaseService
    {
        public IReadOnlyList<IPurchaseData> Purchases {get; }
        public IEnumerator HandlePurchase(string id, Action<bool> callback);
        public IEnumerator Initialize();
    }
}