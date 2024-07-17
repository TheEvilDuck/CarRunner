using System;
using System.Collections.Generic;

namespace Common.Data
{
    public interface IPlayerData
    {
        public IEnumerable<string> AvailableLevels {get;}
        public IEnumerable<string> PassedLevels {get;}
        public string SelectedLevel {get;}
        public int Coins {get;}
        public DateTime WatchShopAdLastTime {get;}
        public event Action<int> coinsChanged;
        public void SaveSelectedLevel(string levelId);
        public void SaveWatchAdLastTime();
        public void AddPassedLevel(string levelId);
        public void AddAvailableLevel(string levelId);
        public bool LoadProgressOfLevels();
        public void AddCoins(int coins);
        public bool SpendCoins(int coins);
    }

}