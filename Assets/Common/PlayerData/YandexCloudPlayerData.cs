using System;
using System.Collections.Generic;
using Common.Reactive;
using YG;
using YG.Utils.LB;

namespace Common.Data
{
    public class YandexCloudPlayerData : IPlayerData
    {
        public event Action<int> coinsChanged;

        private List<string> _availableLevels = YandexGame.savesData.AvailableLevels;
        private List<string> _passedLevels = YandexGame.savesData.PassedLevels;
        private LBData _currentLBData;
        private bool _newLBLoaded;
        private float _lastLeaderBoardCall;
        private Observable<string> _savedPreferedLanguage;

        public IEnumerable<string> AvailableLevels => _availableLevels;
        public IEnumerable<string> PassedLevels => _passedLevels;
        public string SelectedLevel => YandexGame.savesData.SelectedLevel;
        public int Coins => YandexGame.savesData.Coins;
        public int MaxFallTries => YandexGame.savesData.MaxFallTries;
        public DateTime WatchShopAdLastTime
        {
            get
            {
                if (DateTime.TryParse(YandexGame.savesData.WatchShopAdLastTime, out var result))
                {
                    return result;
                }

                return DateTime.MinValue;
            }
        }
        public bool IsTutorialComplete => YandexGame.savesData.IsTutorialComplete;

        public IReadonlyObservable<string> SavedPreferdLanguage => _savedPreferedLanguage;

        public YandexCloudPlayerData()
        {
            _savedPreferedLanguage = new Observable<string>();
            LoadLanguage();
            _savedPreferedLanguage.changed += (language) =>
            {
                YandexGame.savesData.savedLanguage = language;
                YandexGame.SaveProgress();
            };
        }

        public void TutorialCmplete()
        {
            YandexGame.savesData.IsTutorialComplete = true;
            YandexGame.SaveProgress();
        }

        public void AddAvailableLevel(string levelId)
        {
            if (_availableLevels.Contains(levelId))
                return;

            _availableLevels.Add(levelId);
            YandexGame.SaveProgress();
        }

        public void AddCoins(int coins)
        {
            if (coins <= 0)
                throw new ArgumentOutOfRangeException($"Coins count must be positive, you passed {coins}");

            YandexGame.savesData.Coins += coins;
            YandexGame.SaveProgress();
            coinsChanged?.Invoke(YandexGame.savesData.Coins);
        }

        public void AddOrSubtractFallTries(int fallTries)
        {
            if (fallTries == 0)
                throw new ArgumentOutOfRangeException($"FallTries count must be positive or negative");

            if (fallTries < 0)
            {
                if (MaxFallTries - fallTries < 0)
                    throw new ArgumentOutOfRangeException("You don't have enough FallTries");

                YandexGame.savesData.MaxFallTries += fallTries;
                YandexGame.SaveProgress();
            }

            if (fallTries > 0)
            {
                YandexGame.savesData.MaxFallTries += fallTries;
                YandexGame.SaveProgress();
            }
        }

        public void AddPassedLevel(string levelId)
        {
            if (_passedLevels.Contains(levelId))
                return;

            _passedLevels.Add(levelId);
            YandexGame.SaveProgress();
        }

        public bool LoadProgressOfLevels() => YandexGame.SDKEnabled;

        public void SaveSelectedLevel(string levelId)
        {
            YandexGame.savesData.SelectedLevel = levelId;
            YandexGame.SaveProgress();
        }

        public void SaveWatchAdLastTime()
        {
            YandexGame.savesData.WatchShopAdLastTime = DateTime.Now.ToString();
            YandexGame.SaveProgress();
        }

        public bool SpendCoins(int coins)
        {
            if (coins <= 0)
                throw new ArgumentOutOfRangeException($"Coins count must be positive, you passed {coins}");

            if (coins > Coins)
                return false;

            YandexGame.savesData.Coins -= coins;
            YandexGame.SaveProgress();
            coinsChanged?.Invoke(YandexGame.savesData.Coins);
            return true;
        }

        public void SaveLanguage(string language)
        {
            _savedPreferedLanguage.Value = language;
        }

        private void LoadLanguage()
        {
            if (string.IsNullOrEmpty(YandexGame.savesData.savedLanguage))
                _savedPreferedLanguage.Value = YandexGame.EnvironmentData.language;
            else
                _savedPreferedLanguage.Value = YandexGame.savesData.savedLanguage;
        }
    }
}