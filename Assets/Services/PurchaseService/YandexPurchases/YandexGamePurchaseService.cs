using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Services.PurchaseService.YandexPurchases
{
    public class YandexGamePurchaseService: IPurchaseService, IDisposable
    {
        private readonly List<PurchaseData> _purchases = new List<PurchaseData>();
        private readonly List<string> _successfulPurchasesBuffer = new List<string>();
        private readonly List<string> _failedPurchasesBuffer = new List<string>();

        private bool _paymentsGot = false;
        
        public IReadOnlyList<IPurchaseData> Purchases => _purchases;

        public IEnumerator HandlePurchase(string id, Action<bool> callback)
        {
            YandexGame.BuyPayments(id);
            yield return new WaitUntil(() => IsPurchaseProceeded(id));
            
            callback(HandlePurchaseStatus(id));
        }

        public IEnumerator Initialize()
        {
            YandexGame.InitPayments();
            YandexGame.GetPayments();
            YandexGame.GetPaymentsEvent += OnPaymentsGot;
            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent += OnPurchaseFailedEvent;
            
            yield return new WaitUntil(() => _paymentsGot);

            foreach (var purchaseData in _purchases) 
                yield return purchaseData.Load();
            
            for (int i = _purchases.Count - 1; i >= 0; i--)
                if (_purchases[i].Loaded == false)
                    _purchases.RemoveAt(i);
            
            YandexGame.ConsumePurchases();
        }

        public void Dispose()
        {
            YandexGame.GetPaymentsEvent -= OnPaymentsGot;
            YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent -= OnPurchaseFailedEvent;
        }

        private void OnPaymentsGot()
        {
            YandexGame.GetPaymentsEvent -= OnPaymentsGot;

            var purchases = YandexGame.purchases;

            foreach (var purchase in purchases)
            {
                PurchaseData purchaseData = new PurchaseData(
                    purchase.id, 
                    purchase.price,
                    purchase.currencyImageURL, 
                    purchase.imageURI);
                _purchases.Add(purchaseData);
            }

            _paymentsGot = true;
        }
        
        private void OnPurchaseSuccessEvent(string id) => _successfulPurchasesBuffer.Add(id);
        private void OnPurchaseFailedEvent(string obj) => _failedPurchasesBuffer.Add(obj);

        private bool IsPurchaseProceeded(string id)
            => _successfulPurchasesBuffer.Contains(id) || _failedPurchasesBuffer.Contains(id);

        private bool HandlePurchaseStatus(string id)
        {
            if (_successfulPurchasesBuffer.Contains(id))
            {
                _successfulPurchasesBuffer.Remove(id);
                return true;
            }

            if (_failedPurchasesBuffer.Contains(id))
            {
                _failedPurchasesBuffer.Remove(id);
                return false;
            }

            return false;
        }
    }
}