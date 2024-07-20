using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Data
{
    public class PlayerDataPlayerPrefs : IPlayerData
    {
        private const string PREFS_SELECTED_LEVEL = "PLAYERPREFS_SELECTED_LEVEL";
        private const string PREFS_PROGRESS_OF_LEVELS = "PLAYERPREFS_PROGRESS_OF_LEVELS";
        private const string PREFS_COINS = "PLAYEPREFS_COINS";
        private const string PREFS_TIME_RECORD = "PLAYEPREFS_TIME_RECORD";
        private const string PREFS_WATCH_AD_LAST_DATE= "PLAYERPREFS_WATCH_AD_LAST_DATE";
        private const int COINS_DEFAULT_VALUE = 1000;

        private ProgressOfLevels _progressOfLvls;

        public event Action<int> coinsChanged;

        public IEnumerable<string> AvailableLevels => _progressOfLvls.AvailableLevels;
        public IEnumerable<string> PassedLevels => _progressOfLvls.PassedLevels;
        public string SelectedLevel => PlayerPrefs.GetString(PREFS_SELECTED_LEVEL);
        public int Coins => PlayerPrefs.GetInt(PREFS_COINS, COINS_DEFAULT_VALUE);
        public float RecordTime
        {
            get
            {
                if(PlayerPrefs.HasKey(PREFS_TIME_RECORD))
                    return PlayerPrefs.GetFloat(PREFS_TIME_RECORD);
                else
                    return 0;
            }
        }

        public DateTime WatchShopAdLastTime
        {
            get
            {
                var dateTimeString = PlayerPrefs.GetString(PREFS_WATCH_AD_LAST_DATE, DateTime.MinValue.ToString());
                var dateTime = DateTime.Parse(dateTimeString);
                return dateTime;
            }
        }

        private void SaveProgressOfLevels()
        {
            string progressOfLevels = JsonUtility.ToJson(_progressOfLvls);
            PlayerPrefs.SetString(PREFS_PROGRESS_OF_LEVELS, progressOfLevels);
        }

        public void SaveSelectedLevel(string levelId) => PlayerPrefs.SetString(PREFS_SELECTED_LEVEL, levelId);

        public void AddPassedLevel(string levelId)
        {
            if (_progressOfLvls.PassedLevels.Contains(levelId))
                return;
            else
            {
                _progressOfLvls.PassedLevels.Add(levelId);
                SaveProgressOfLevels();
            }
        }

        public void AddAvailableLevel(string levelId)
        {
            if (_progressOfLvls.AvailableLevels.Contains(levelId))
                return;
            else
            {
                _progressOfLvls.AvailableLevels.Add(levelId);
                SaveProgressOfLevels();
            }
        }

        public bool LoadProgressOfLevels()
        {
            string progressOfLevels;
            if (PlayerPrefs.HasKey(PREFS_PROGRESS_OF_LEVELS))
            {
                progressOfLevels = PlayerPrefs.GetString(PREFS_PROGRESS_OF_LEVELS);
                _progressOfLvls = JsonUtility.FromJson<ProgressOfLevels>(progressOfLevels);
                return true;
            }
            else
            {
                _progressOfLvls = new ProgressOfLevels();
                return false;
            }
        }

        public void AddCoins(int coins)
        {
            if (coins <= 0)
                throw new ArgumentOutOfRangeException($"Coins count must be positive, you passed {coins}");

            PlayerPrefs.SetInt(PREFS_COINS, Coins + coins);
            coinsChanged?.Invoke(Coins);
        }

        public bool SpendCoins(int coins)
        {
            if (coins <= 0)
                throw new ArgumentOutOfRangeException($"Coins count must be positive, you passed {coins}");

            if (coins > Coins)
                return false;

            PlayerPrefs.SetInt(PREFS_COINS, Coins - coins);
            coinsChanged?.Invoke(Coins);
            return true;
        }

        public void SaveWatchAdLastTime()
        {
            PlayerPrefs.SetString(PREFS_WATCH_AD_LAST_DATE, DateTime.Now.ToString());
        }

        public void SaveRecordTime(float time)
        {
            if (PlayerPrefs.HasKey(PREFS_TIME_RECORD))
            {
                float currentRecord = RecordTime;
                if(time > currentRecord)
                    PlayerPrefs.SetFloat(PREFS_TIME_RECORD, time);
            }
            else
            {
                PlayerPrefs.SetFloat(PREFS_TIME_RECORD, time);
            }
        }

        [Serializable]
        private class ProgressOfLevels
        {
            public List<string> PassedLevels;
            public List<string> AvailableLevels;

            public ProgressOfLevels()
            {
                PassedLevels = new List<string>();
                AvailableLevels = new List<string>();
            }
        }
    }
}
