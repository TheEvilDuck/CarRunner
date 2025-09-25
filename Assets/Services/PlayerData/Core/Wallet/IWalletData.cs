namespace Services.PlayerData.Core.Wallet
{
    public interface IWalletData: IData
    {
        public int Coins { get; }
    }
}