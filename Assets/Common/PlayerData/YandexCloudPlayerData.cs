using Common;
using System;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using YG;

namespace Common.Data
{
    public class YandexCloudPlayerData : IPlayerData
    {
        private List<string> _availableLevels = YandexGame.savesData.AvailableLevels;
        private List<string> _passedLevels = YandexGame.savesData.PassedLevels;

        public event Action<int> coinsChanged;

        public IEnumerable<string> AvailableLevels => _availableLevels;
        public IEnumerable<string> PassedLevels => _passedLevels;
        public string SelectedLevel => YandexGame.savesData.SelectedLevel;
        public int Coins => YandexGame.savesData.Coins;
        public DateTime WatchShopAdLastTime => YandexGame.savesData.WatchShopAdLastTime;
        public float RecordTime => YandexGame.savesData.RecordTime;

        public void AddAvailableLevel(string levelId)
        {
            if (_availableLevels.Contains(levelId))
                return;
            else
            {
                _availableLevels.Add(levelId);
                YandexGame.SaveProgress();
            }
        }

        public void AddCoins(int coins)
        {
            if (coins <= 0)
                throw new ArgumentOutOfRangeException($"Coins count must be positive, you passed {coins}");

            YandexGame.savesData.Coins += coins;
            coinsChanged?.Invoke(YandexGame.savesData.Coins);
        }

        public void AddPassedLevel(string levelId)
        {
            if (_passedLevels.Contains(levelId))
                return;
            else
            {
                _passedLevels.Add(levelId);
                YandexGame.SaveProgress();
            }
        }

        public bool LoadProgressOfLevels() => YandexGame.SDKEnabled;

        public void SaveSelectedLevel(string levelId)
        {
            YandexGame.savesData.SelectedLevel = levelId;
            YandexGame.SaveProgress();
        }

        public void SaveRecordTime(float time)
        {
            float currentRecord = YandexGame.savesData.RecordTime;
            if (time > currentRecord)
                YandexGame.savesData.RecordTime = time;
        }

        public void SaveWatchAdLastTime()
        {
            YandexGame.savesData.WatchShopAdLastTime = DateTime.Now;
        }

        public bool SpendCoins(int coins)
        {
            if (coins <= 0)
                throw new ArgumentOutOfRangeException($"Coins count must be positive, you passed {coins}");

            if (coins > Coins)
                return false;

            YandexGame.savesData.Coins -= coins;
            coinsChanged?.Invoke(YandexGame.savesData.Coins);
            return true;
        }
    }
}

