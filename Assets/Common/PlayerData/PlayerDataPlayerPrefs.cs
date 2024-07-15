using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Data
{
    public class PlayerDataPlayerPrefs: IPlayerData
    {
        private const string PREFS_SELECTED_LEVEL = "PLAYERPREFS_SELECTED_LEVEL";
        private const string PREFS_PROGRESS_OF_LEVELS = "PLAYERPREFS_PROGRESS_OF_LEVELS";
        private const string PREFS_COINS = "PLAYEPREFS_COINS";
        private const string PREFS_WATCH_AD_LAST_DAY = "PLAYERPREFS_WATCH_AD_LAST_DAY";
        private const string PREFS_WATCH_AD_LAST_TIME = "PLAYERPREFS_WATCH_AD_LAST_TIME";
        private const int COINS_DEFAULT_VALUE = 1000;

        public event Action<int> coinsChanged;


        private ProgressOfLevels _progressOfLvls;
        public IEnumerable<string> AvailableLevels => _progressOfLvls.AvailableLevels;
        public IEnumerable<string> PassedLevels => _progressOfLvls.PassedLevels;

        public string SelectedLevel => PlayerPrefs.GetString(PREFS_SELECTED_LEVEL);

        public int Coins => PlayerPrefs.GetInt(PREFS_COINS, COINS_DEFAULT_VALUE);
        public DateTime WatchShopAdLastTime
        {
            get
            {
                int day = DateTime.Now.Day;
                TimeSpan timeSpan = DateTime.Now.TimeOfDay;

                if (DateTime.TryParse(PlayerPrefs.GetString(PREFS_WATCH_AD_LAST_DAY), out DateTime resultDay))
                    day = resultDay.Day;

                if (DateTime.TryParse(PlayerPrefs.GetString(PREFS_WATCH_AD_LAST_TIME), out DateTime resultTime))
                    timeSpan = resultDay.TimeOfDay;

                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
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

        private void SaveProgressOfLevels()
        {
            string progressOfLevels = JsonUtility.ToJson(_progressOfLvls);
            PlayerPrefs.SetString(PREFS_PROGRESS_OF_LEVELS, progressOfLevels);
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
            PlayerPrefs.SetString(PREFS_WATCH_AD_LAST_TIME, DateTime.Now.ToLongTimeString());
            PlayerPrefs.SetString(PREFS_WATCH_AD_LAST_DAY, DateTime.Now.ToLongDateString());
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
