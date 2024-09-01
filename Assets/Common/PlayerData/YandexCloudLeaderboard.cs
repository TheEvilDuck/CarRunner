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
        private const float LEADERBOARD_CALL_COOLDOWN = 1f;
        private Dictionary<string, LBData> _cashedLeaderboard;
        private Queue<LeaderboardCall> _callLeaderboards;
        private List<string> _queueCallsIds;

        private float _lastLeaderBoardCall;

        public YandexCloudLeaderboard()
        {
            _cashedLeaderboard = new Dictionary<string, LBData>();
            _callLeaderboards = new Queue<LeaderboardCall>();
            _queueCallsIds = new List<string>();

            YandexGame.onGetLeaderboard += OnLeaderBoardLoaded;
        }

        public void Dispose()
        {
            YandexGame.onGetLeaderboard -= OnLeaderBoardLoaded;
        }

        public async Awaitable<LBData> GetLeaderBoard(string levelId)
        {
            CallLeaderboard(levelId);

            while (!_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId))
                await Awaitable.WaitForSecondsAsync(1);

            return _cashedLeaderboard[LEADERBOARD_KEY + levelId];
        }

        public async Awaitable<float> GetLevelRecord(string levelId)
        {
            CallLeaderboard(levelId);

            while (!_cashedLeaderboard.ContainsKey(LEADERBOARD_KEY + levelId))
                await Awaitable.WaitForSecondsAsync(1);

            return _cashedLeaderboard[LEADERBOARD_KEY + levelId].thisPlayer.score;
        }

        public async Awaitable SaveLevelRecord(string levelId, float recordTime)
        {
            float previous = await GetLevelRecord(levelId);

            if (recordTime * 1000f > previous)
                YandexGame.NewLBScoreTimeConvert(LEADERBOARD_KEY + levelId, recordTime);
        }

        public void Tick(float deltaTime)
        {
            if (Time.time - _lastLeaderBoardCall < LEADERBOARD_CALL_COOLDOWN)
                return;

            if (!_callLeaderboards.TryDequeue(out var leaderboardCall))
                return;
            
            leaderboardCall.call.Invoke();
            _queueCallsIds.Remove(leaderboardCall.leaderBoardId);
            _lastLeaderBoardCall = Time.time;
        }

        private void CallLeaderboard(string levelId)
        {
            if (_queueCallsIds.Contains(levelId))
                return;

            LeaderboardCall leaderboardCall = new LeaderboardCall()
            {
                call = () => YandexGame.GetLeaderboard(LEADERBOARD_KEY + levelId, 10, 3, 3, "small"),
                leaderBoardId = LEADERBOARD_KEY + levelId
            };

            _callLeaderboards.Enqueue(leaderboardCall);
            _queueCallsIds.Add(LEADERBOARD_KEY + levelId);
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
