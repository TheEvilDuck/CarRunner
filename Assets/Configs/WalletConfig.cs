using Services.PlayerData.Core.Wallet;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "WalletConfig", menuName = "Configs/WalletConfig")]
    public class WalletConfig: ScriptableObject, IWalletConfig
    {
        [field: SerializeField] public int StartCoins { get; private set; }
    }
}