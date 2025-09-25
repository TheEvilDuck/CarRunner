using System;
using System.Collections;
using Common.CoroutinePerformer;
using Common.Reactive;
using UnityEngine;

namespace Services.PlayerData.Core.Wallet
{
    public class WalletService: DataService<IWalletData>, IDisposable, IWalletService
    {
        public override event Action DataChanged;

        private readonly IWalletConfig _config;
        
        private Observable<int> _coins;
        
        public IReadonlyObservable<int> Coins => _coins;
        
        public WalletService(
            IDataProvider<IWalletData> dataProvider, 
            ICoroutinePerformer coroutinePerformer, 
            IWalletConfig config) : base(dataProvider, coroutinePerformer)
        {
            _config = config;
        }
        
        protected override IEnumerator InitializeInternal(IWalletData data)
        {
            _coins = new Observable<int>(data.Coins);
            _coins.changed += OnCoinsChanged;
            yield break;
        }

        public void Dispose() => _coins.changed -= OnCoinsChanged;

        public bool IsEnough(int amount)
        {
            ValidateCoinsAmount(amount);
            return _coins.Value >= amount;
        }

        public bool SpendCoins(int amount)
        {
            ValidateCoinsAmount(amount);
            
            if (!IsEnough(amount))
                return false;

            _coins.Value -= amount;

            return true;
        }

        public void AddCoins(int amount)
        {
            ValidateCoinsAmount(amount);
            _coins.Value += amount;
        }

        protected override IEnumerator LoadDefaultData(DataLoadCallback<IWalletData> callback)
        {
            callback?.Invoke(true, new WalletData(_config.StartCoins));
            yield break;
        }

        protected override IWalletData GetData() => new WalletData(_coins.Value);

        private void ValidateCoinsAmount(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException($"Coins amount can't be less or equal to zero, you passed {amount}");
        }

        private void OnCoinsChanged(int coins) => DataChanged?.Invoke();

        private class WalletData : IWalletData
        {
            public int Coins { get; }
            
            public WalletData(int coins)
            {
                Coins = coins;
            }
        }
    }
}