using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Utils.LB;

namespace Common.Data
{
    public class YandexCloudLeaderboard : ILeaderBoardData, IDisposable, ITickable
    {
        public const string LEADERBOARD_KEY = "yandexLevelRecords";
        private const float LEADERBOARD_CALL_COOLDOWN = 300f;
        private const float TIME_BETWEEN_CALLS = 2f;
        private const int MAX_CALLS_IN_COOLDOWN = 20;
        private const float CALL_TIMEOUT = 10f;
        private Dictionary<string, LBData> _cashedLeaderboard;
        private Queue<LeaderboardCall> _callLeaderboards;
        private Dictionary<string, float> _queueCallsTimeData;
        private int _lastCooldownCalls;

        private float _lastLeaderBoardCall;

        public YandexCloudLeaderboard()
        {
            _cashedLeaderboard = new Dictionary<string, LBData>();
            _callLeaderboards = new Queue<LeaderboardCall>();
            _queueCallsTimeData = new Dictionary<string, float>();

            YandexGame.onGetLeaderboard += OnLeaderBoardLoaded;
        }

        public void Dispose()
        {
            YandexGame.onGetLeaderboard -= OnLeaderBoardLoaded;
        }

        public async Awaitable<LBData> GetLeaderBoard(string levelId)
        {
            if (!_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId))
                CallLeaderboard(levelId);

            while 
            (
            !_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId) && 
            (!_queueCallsTimeData.ContainsKey(LEADERBOARD_KEY + levelId) || 
            Time.time - _queueCallsTimeData[LEADERBOARD_KEY + levelId] < CALL_TIMEOUT)
            )
                await Awaitable.WaitForSecondsAsync(1);

            if (!_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId))
            {
                Debug.LogError($"Can't load leaderboard: {LEADERBOARD_KEY + levelId}. Timeout!");
                return null;
            }

            return _cashedLeaderboard[LEADERBOARD_KEY + levelId];
        }

        public async Awaitable<float> GetLevelRecord(string levelId)
        {
            if (!_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId))
                CallLeaderboard(levelId);

            while (!_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId))
                await Awaitable.WaitForSecondsAsync(1);

            return _cashedLeaderboard[LEADERBOARD_KEY + levelId].thisPlayer.score;
        }

        public async Awaitable SaveLevelRecord(string levelId, float recordTime)
        {
            if (!_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId))
                await GetLevelRecord(levelId);

            float previous = _cashedLeaderboard[LEADERBOARD_KEY + levelId].thisPlayer.score;

            if (recordTime * 1000f > previous)
            {
                _cashedLeaderboard[LEADERBOARD_KEY + levelId].thisPlayer.score = (int) (recordTime * 1000f);
                YandexGame.NewLBScoreTimeConvert(LEADERBOARD_KEY + levelId, recordTime);
                CallLeaderboard(levelId);
            }
        }

        public void Tick(float deltaTime)
        {
            if (_lastCooldownCalls >= MAX_CALLS_IN_COOLDOWN && Time.time - _lastLeaderBoardCall < LEADERBOARD_CALL_COOLDOWN)
                return;
            else if (Time.time - _lastLeaderBoardCall >= LEADERBOARD_CALL_COOLDOWN)
                _lastCooldownCalls = 0;

            if (Time.time - _lastLeaderBoardCall < TIME_BETWEEN_CALLS)
                return;

            if (!_callLeaderboards.TryDequeue(out var leaderboardCall))
                return;
            
            _lastCooldownCalls ++;
            leaderboardCall.call.Invoke();
            _queueCallsTimeData.Remove(leaderboardCall.leaderBoardId);
            _lastLeaderBoardCall = Time.time;
        }

        private void CallLeaderboard(string levelId)
        {
            if (_queueCallsTimeData.ContainsKey(LEADERBOARD_KEY + levelId))
                return;

            LeaderboardCall leaderboardCall = new LeaderboardCall()
            {
                call = () => YandexGame.GetLeaderboard(LEADERBOARD_KEY + levelId, 10, 3, 3, "small"),
                leaderBoardId = LEADERBOARD_KEY + levelId
            };

            _callLeaderboards.Enqueue(leaderboardCall);
            _queueCallsTimeData.Add(LEADERBOARD_KEY + levelId, Time.time);
        }

        private void OnLeaderBoardLoaded(LBData lBData)
        {
            _cashedLeaderboard.TryAdd(lBData.technoName, lBData);
        }

        private struct LeaderboardCall
        {
            public string leaderBoardId;
            public Action call;
        }
    }
}
