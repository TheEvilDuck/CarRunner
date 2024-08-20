using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Data
{
    public interface IPlayerData
    {
        public IEnumerable<string> AvailableLevels {get;}
        public IEnumerable<string> PassedLevels {get;}
        public string SelectedLevel {get;}
        public int Coins {get;}
        public bool IsTutorialComplete { get;}
        public DateTime WatchShopAdLastTime {get;}
        public string SavedPreferedLanguage {get;}
        public event Action<int> coinsChanged;
        public void SaveSelectedLevel(string levelId);
        public void SaveWatchAdLastTime();
        public void AddPassedLevel(string levelId);
        public void AddAvailableLevel(string levelId);
        public bool LoadProgressOfLevels();
        public void AddCoins(int coins);
        public bool SpendCoins(int coins);
        public void TutorialCmplete();
        public void SaveLanguage(string language);
        public Awaitable SaveLevelRecord(string levelId, float recordTime);
        public Awaitable<float> GetLevelRecord(string levelId);
    }

}