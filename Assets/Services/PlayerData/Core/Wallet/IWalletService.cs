namespace Services.PlayerData.Core.Wallet
{
    public interface IWalletService
    {
        public bool IsEnough(int amount);
        public bool SpendCoins(int amount);
        public void AddCoins(int amount);
    }
}