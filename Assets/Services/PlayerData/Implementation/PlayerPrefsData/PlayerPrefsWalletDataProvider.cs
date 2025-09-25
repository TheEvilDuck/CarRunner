using System.Collections;
using Common.CoroutineCallbacks;
using Services.PlayerData.Core;
using Services.PlayerData.Core.Wallet;
using UnityEngine;

namespace Services.PlayerData.Implementation.PlayerPrefsData
{
    public class PlayerPrefsWalletDataProvider: IDataProvider<IWalletData>
    {
        private const string COINS_KEY = "PLAYEPREFS_COINS";
        
        public IEnumerator Load(DataLoadCallback<IWalletData> callback)
        {
            if (!PlayerPrefs.HasKey(COINS_KEY))
            {
                callback?.Invoke(false, null);
                yield break;
            }

            WalletData walletData = new WalletData(PlayerPrefs.GetInt(COINS_KEY));
            
            callback?.Invoke(true, walletData);
            yield break;
        }

        public IEnumerator Save(IWalletData data, SuccessCallback callback)
        {
            PlayerPrefs.SetInt(COINS_KEY, data.Coins);
            callback?.Invoke(true);
            yield break;
        }

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