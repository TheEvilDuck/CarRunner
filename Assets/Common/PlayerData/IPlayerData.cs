using System;
using System.Collections.Generic;
using Common.Reactive;
using UnityEngine;

namespace Common.Data
{
    public interface IPlayerData
    {
        public IEnumerable<string> AvailableLevels { get; }
        public IEnumerable<string> PassedLevels { get; }
        public string SelectedLevel { get; }
        public int Coins { get; }
        public int MaxFallTries { get; }
        public bool IsTutorialComplete { get; }
        public DateTime WatchShopAdLastTime { get; }
        public IReadonlyObservable<string> SavedPreferdLanguage { get; }
        public event Action<int> coinsChanged;
        public void SaveSelectedLevel(string levelId);
        public void SaveWatchAdLastTime();
        public void AddPassedLevel(string levelId);
        public void AddAvailableLevel(string levelId);
        public bool LoadProgressOfLevels();
        public void AddCoins(int coins);
        public void AddOrSubtractFallTries(int fallTries);
        public bool SpendCoins(int coins);
        public void TutorialCmplete();
        public void SaveLanguage(string language);
    }
}