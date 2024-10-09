using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Utils.LB;
using System.Linq;

namespace Common.Data
{
    public class YandexCloudLeaderboard : ILeaderBoardData, IDisposable, ITickable
    {
        public const string LEADERBOARD_KEY = "yandexLevelRecords";
        public const string COINS_LEADERBOARD_KEY = "coins";
        private const float LEADERBOARD_CALL_COOLDOWN = 300f;
        private const float TIME_BETWEEN_CALLS = 2f;
        private const float TIME_BETWEEN_SAVE_CALLS = 1.5f;
        private const int MAX_CALLS_IN_COOLDOWN = 20;
        private const float CALL_TIMEOUT = 10f;

        public event Action<LBData> leaderboardUpdated;
        private Dictionary<string, LBData> _cashedLeaderboard;
        private Queue<LeaderboardCall> _callLeaderboards;
        private Queue<Action> _saveLeaderboardCalls;
        private Dictionary<string, float> _queueCallsTimeData;
        private int _lastCooldownCalls;

        private float _lastLeaderBoardCall;
        private float _lastLeaderBoardSaveCall;

        public YandexCloudLeaderboard()
        {
            _cashedLeaderboard = new Dictionary<string, LBData>();
            _callLeaderboards = new Queue<LeaderboardCall>();
            _queueCallsTimeData = new Dictionary<string, float>();
            _saveLeaderboardCalls = new Queue<Action>();

            YandexGame.onGetLeaderboard += OnLeaderBoardLoaded;
        }

        public void Dispose()
        {
            YandexGame.onGetLeaderboard -= OnLeaderBoardLoaded;
        }

        public async Awaitable<LBData> GetLeaderBoard(string levelId)
        {
            if (!_cashedLeaderboard.ContainsKey(GetLeaderboardId(levelId)))
                CallLeaderboard(levelId);

            while 
            (
            !_cashedLeaderboard.ContainsKey(GetLeaderboardId(levelId)) && 
            _queueCallsTimeData.ContainsKey(GetLeaderboardId(levelId)) && 
            Time.time - _queueCallsTimeData[GetLeaderboardId(levelId)] < CALL_TIMEOUT
            )
                await Awaitable.WaitForSecondsAsync(1);

            if (!_cashedLeaderboard.ContainsKey(GetLeaderboardId(levelId)))
            {
                Debug.LogError($"Can't load leaderboard: {GetLeaderboardId(levelId)}. Timeout!");
                return null;
            }

            return _cashedLeaderboard[GetLeaderboardId(levelId)];
        }

        public async Awaitable<float> GetLevelRecord(string levelId)
        {
            if (!_cashedLeaderboard.ContainsKey(GetLeaderboardId(levelId)))
                CallLeaderboard(levelId);

            float timeout = 0;

            while (!_cashedLeaderboard.ContainsKey(GetLeaderboardId(levelId)) && timeout <= CALL_TIMEOUT)
            {
                await Awaitable.WaitForSecondsAsync(1);
                timeout += 1f;
            }

            if (_cashedLeaderboard.ContainsKey(GetLeaderboardId(levelId)))
                return _cashedLeaderboard[GetLeaderboardId(levelId)].thisPlayer.score;

            Debug.Log($"Leaderboard is not loaded");

            return float.MinValue;
        }

        public async Awaitable SaveLevelRecord(string levelId, float recordTime)
        {
            float previous = await GetLevelRecord(levelId);

            if (recordTime * 1000f > previous || previous <= 0 && recordTime > 0)
            {
                SaveThisPlayerLocal(levelId, (int) (recordTime * 1000f));
                
                _saveLeaderboardCalls.Enqueue(() => YandexGame.NewLBScoreTimeConvert(GetLeaderboardId(levelId), recordTime));

                CallLeaderboard(levelId);
                
                _cashedLeaderboard[GetLeaderboardId(levelId)] = LocalSortLBData(_cashedLeaderboard[GetLeaderboardId(levelId)]);
            }
        }

        public void SaveCoins(int coins) => _saveLeaderboardCalls.Enqueue(() => YandexGame.NewLeaderboardScores(COINS_LEADERBOARD_KEY, coins));

        private void SaveThisPlayerLocal(string levelId, int score)
        {
            if (!_cashedLeaderboard.ContainsKey(GetLeaderboardId(levelId)))
            {
                LBData lBData = new LBData();
                lBData.technoName = GetLeaderboardId(levelId);
                lBData.thisPlayer = new LBThisPlayerData();
                _cashedLeaderboard.Add(GetLeaderboardId(levelId), lBData);
            }

            bool found = false;

            foreach (var data in _cashedLeaderboard[GetLeaderboardId(levelId)].players)
            {
                if (data.uniqueID == YandexGame.playerId)
                {
                    data.score = score;
                    found = true;
                }
            }

            if (!found)
            {
                List<LBPlayerData> newLB = new List<LBPlayerData>(_cashedLeaderboard[GetLeaderboardId(levelId)].players);

                LBPlayerData newData = new LBPlayerData();
                newData.uniqueID = YandexGame.playerId;
                newData.name = YandexGame.playerName;
                newData.photo = YandexGame.playerPhoto;
                newData.score = score;
                newLB.Add(newData);

                _cashedLeaderboard[GetLeaderboardId(levelId)].players = newLB.ToArray();
            }

            _cashedLeaderboard[GetLeaderboardId(levelId)].thisPlayer.score = score;
        }

        public void Tick(float deltaTime)
        {
            if (Time.time - _lastLeaderBoardSaveCall >= TIME_BETWEEN_SAVE_CALLS && _saveLeaderboardCalls.TryDequeue(out var saveCall))
            {
                saveCall?.Invoke();
                _lastLeaderBoardSaveCall = Time.time;
            }

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
            if (_queueCallsTimeData.ContainsKey(GetLeaderboardId(levelId)))
                return;

            LeaderboardCall leaderboardCall = new LeaderboardCall()
            {
                call = () => YandexGame.GetLeaderboard(GetLeaderboardId(levelId), 10, 3, 3, "small"),
                leaderBoardId = GetLeaderboardId(levelId)
            };

            _callLeaderboards.Enqueue(leaderboardCall);
            _queueCallsTimeData.Add(GetLeaderboardId(levelId), Time.time);
        }

        private void OnLeaderBoardLoaded(LBData lBData)
        {
            _cashedLeaderboard[lBData.technoName] = lBData;
            leaderboardUpdated?.Invoke(lBData);
        }

        private LBData LocalSortLBData(LBData lBData)
        {
            int countOfPlayersInLB = lBData.players.Count();
            int[] ranksBeforeSort = new int[countOfPlayersInLB];

            for (int i = 0; i < countOfPlayersInLB; i++)
                ranksBeforeSort[i] = lBData.players[i].rank;

            lBData.players = lBData.players.OrderByDescending(player => player.score).ToArray();

            for (int i = 0; i < countOfPlayersInLB; i++)
            {
                lBData.players[i].rank = ranksBeforeSort[i];
                
                if(YandexGame.playerId == lBData.players[i].uniqueID)
                    lBData.thisPlayer.rank = ranksBeforeSort[i];
            }
            
            return lBData;
        }

        public string GetLeaderboardId(string levelId) => LEADERBOARD_KEY + levelId;

        

        private struct LeaderboardCall
        {
            public string leaderBoardId;
            public Action call;
        }
    }
}
