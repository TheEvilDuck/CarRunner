using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Utils.LB;

namespace Common.Data
{
    public class YandexCloudPlayerData : IPlayerData, IDisposable
    {
        public const string LEADERBOARD_KEY = "YANDEX_LEVEL_RECORDS_";
        private const float LEADERBOARD_CALL_COOLDOWN = 1f;

        public event Action<int> coinsChanged;

        private List<string> _availableLevels = YandexGame.savesData.AvailableLevels;
        private List<string> _passedLevels = YandexGame.savesData.PassedLevels;
        private LBData _currentLBData;
        private bool _newLBLoaded;
        private float _lastLeaderBoardCall;

        public IEnumerable<string> AvailableLevels => _availableLevels;
        public IEnumerable<string> PassedLevels => _passedLevels;
        public string SelectedLevel => YandexGame.savesData.SelectedLevel;
        public int Coins => YandexGame.savesData.Coins;
        public DateTime WatchShopAdLastTime => YandexGame.savesData.WatchShopAdLastTime;
        public bool IsTutorialComplete => YandexGame.savesData.IsTutorialComplete;

        public YandexCloudPlayerData()
        {
            YandexGame.onGetLeaderboard += OnLeaderboardGot;
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

        public void Dispose()
        {
            YandexGame.onGetLeaderboard -= OnLeaderboardGot;
        }

        public async Awaitable SaveLevelRecord(string levelId, float recordTime)
        {
            float previous = await GetLevelRecord(levelId);

            if (recordTime > previous)
                YandexGame.NewLBScoreTimeConvert(LEADERBOARD_KEY + levelId, recordTime);
        }

        public async Awaitable<float> GetLevelRecord(string levelId)
        {
            _newLBLoaded = false;

            if (Time.time - _lastLeaderBoardCall < LEADERBOARD_CALL_COOLDOWN)
                await Awaitable.WaitForSecondsAsync(LEADERBOARD_CALL_COOLDOWN - (Time.time - _lastLeaderBoardCall));

            YandexGame.GetLeaderboard(LEADERBOARD_KEY + levelId, 10, 3, 3, "small");

            while (!_newLBLoaded)
                await Awaitable.WaitForSecondsAsync(1f);

            return _currentLBData.thisPlayer.score;
        }

        private void OnLeaderboardGot(LBData lBData)
        {
            _currentLBData = lBData;
            _newLBLoaded = true;
        }
    }
}