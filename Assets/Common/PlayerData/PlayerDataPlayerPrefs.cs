using System;
using System.Collections.Generic;
using Common.Reactive;
using UnityEngine;

namespace Common.Data
{
    public class PlayerDataPlayerPrefs : IPlayerData
    {
        private const string PREFS_SELECTED_LEVEL = "PLAYERPREFS_SELECTED_LEVEL";
        private const string PREFS_PROGRESS_OF_LEVELS = "PLAYERPREFS_PROGRESS_OF_LEVELS";
        private const string PREFS_COINS = "PLAYEPREFS_COINS";
        private const string PREFS_FALL_TRIES = "PLAYEPREFS_COINS_FALL_TRIES";
        private const string PREFS_TIME_RECORD = "PLAYEPREFS_TIME_RECORD";
        private const string PREFS_WATCH_AD_LAST_DATE = "PLAYERPREFS_WATCH_AD_LAST_DATE";
        private const string PREFS_IS_TUTOR_COMPLETE = "PLAYERPREFS_IS_TUTOR_COMPLETE";
        private const string PREFS_LANGUAGE = "PLAYERPREFS_LANGUAGE";
        private const int COINS_DEFAULT_VALUE = 1000;
        private const int MAX_FALL_TRIES_DEFAULT_VALUE = 1;

        private ProgressOfLevels _progressOfLvls;
        private List<LevelRecord> _levelRecords;
        private Observable<string> _savedPreferedLanguage;

        public event Action<int> coinsChanged;

        public IEnumerable<string> AvailableLevels => _progressOfLvls.AvailableLevels;
        public IEnumerable<string> PassedLevels => _progressOfLvls.PassedLevels;
        public string SelectedLevel => PlayerPrefs.GetString(PREFS_SELECTED_LEVEL);
        public int Coins => PlayerPrefs.GetInt(PREFS_COINS, COINS_DEFAULT_VALUE);
        public int MaxFallTries => PlayerPrefs.GetInt(PREFS_FALL_TRIES, MAX_FALL_TRIES_DEFAULT_VALUE);
        public string SavedPreferedLanguage => PlayerPrefs.GetString(PREFS_LANGUAGE);
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
        public bool IsTutorialComplete
        {
            get
            {
                if (PlayerPrefs.HasKey(PREFS_IS_TUTOR_COMPLETE))
                    return true;
                else 
                    return false;
            }
        }

        public IReadonlyObservable<string> SavedPreferdLanguage => _savedPreferedLanguage;

        public PlayerDataPlayerPrefs()
        {
            LoadProgressOfLevels();
            _savedPreferedLanguage = new Observable<string>();
            LoadLanguage();
            _savedPreferedLanguage.changed += (language) => PlayerPrefs.SetString(PREFS_LANGUAGE, language);
        }

        public void TutorialCmplete()
        {
            if (!PlayerPrefs.HasKey(PREFS_IS_TUTOR_COMPLETE))
            {
                int convertBool = Convert.ToInt32(true);
                PlayerPrefs.SetInt(PREFS_IS_TUTOR_COMPLETE, convertBool);
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

        public void AddOrSubtractFallTries(int fallTries)
        {
            if (fallTries == 0)
                throw new ArgumentOutOfRangeException($"FallTries count must be positive or negative");

            if (fallTries < 0)
            {
                if (MaxFallTries - fallTries < 0)
                    throw new ArgumentOutOfRangeException("You don't have enough FallTries");

                PlayerPrefs.SetInt(PREFS_FALL_TRIES, MaxFallTries + fallTries);
            }

            if (fallTries > 0)
                PlayerPrefs.SetInt(PREFS_FALL_TRIES, MaxFallTries + fallTries);
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

        public async Awaitable SaveLevelRecord(string levelId, float recordTime)
        {
            string recordsJson;
            
            if (PlayerPrefs.HasKey(PREFS_TIME_RECORD))
            {
                recordsJson = PlayerPrefs.GetString(PREFS_PROGRESS_OF_LEVELS);
                _levelRecords = JsonUtility.FromJson<List<LevelRecord>>(recordsJson);
            }
            else
            {
                _levelRecords = new List<LevelRecord>();
            }

            LevelRecord currentLevelRecord = new LevelRecord
            {
                levelId = levelId,
                time = recordTime
            };

            bool found = false;

            foreach (LevelRecord levelRecord in _levelRecords)
            {
                if (string.Equals(levelId, levelRecord.levelId))
                {
                    currentLevelRecord.time = Mathf.Max(levelRecord.time, recordTime);
                    found = true;
                    break;
                }
            }

            if (!found)
                _levelRecords.Add(currentLevelRecord);

            recordsJson = JsonUtility.ToJson(_levelRecords);
            PlayerPrefs.SetString(PREFS_PROGRESS_OF_LEVELS, recordsJson);
        }

        public async Awaitable<float> GetLevelRecord(string levelId)
        {
            string recordsJson;

            if (PlayerPrefs.HasKey(PREFS_TIME_RECORD))
            {
                recordsJson = PlayerPrefs.GetString(PREFS_PROGRESS_OF_LEVELS);
                _levelRecords = JsonUtility.FromJson<List<LevelRecord>>(recordsJson);

                foreach (LevelRecord levelRecord in _levelRecords)
                {
                    if (string.Equals(levelId, levelRecord.levelId))
                    {
                        return levelRecord.time;
                    }
                }
            }

            return 0;
        }

        public void SaveLanguage(string language)
        {
            _savedPreferedLanguage.Value = language;
        }

        private void LoadLanguage()
        {
            if(PlayerPrefs.HasKey(PREFS_LANGUAGE))
            {
                string language = PlayerPrefs.GetString(PREFS_LANGUAGE);
                _savedPreferedLanguage.Value = language;
            }
            else
            {
                //пусть тут пока будет пусто и 
                //будет подгружаться дефолтный (английский) язык 
                //в системе локализации
            }
        }

        private void SaveProgressOfLevels()
        {
            string progressOfLevels = JsonUtility.ToJson(_progressOfLvls);
            PlayerPrefs.SetString(PREFS_PROGRESS_OF_LEVELS, progressOfLevels);
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

        [Serializable]
        private class LevelRecord
        {
            public string levelId;
            public float time;
        }
    }
}
