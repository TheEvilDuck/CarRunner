using System;

namespace Services.PlayerData.SavingStrategy
{
    public interface ISavingStrategy
    {
        public event Action SaveRequested;
    }
}