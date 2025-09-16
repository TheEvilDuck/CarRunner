using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.CoroutinePerformer;
using Common.Tickables;
using Services.SpriteURLLoading;
using UnityEngine;
using YG;
using YG.Utils.LB;

namespace Services.LeaderBoards
{
    public class YandexCloudLeaderboardProvider : ILeaderBoardProvider, IDisposable, ITickable
    {
        private const string LEADERBOARD_KEY = "yandexLevelRecords";
        private const string COINS_LEADERBOARD_KEY = "coins";
        
        private const float LEADERBOARD_CALL_COOLDOWN = 300f;
        private const float TIME_BETWEEN_CALLS = 2f;
        private const float TIME_BETWEEN_SAVE_CALLS = 1.5f;
        private const int MAX_CALLS_IN_COOLDOWN = 20;
        private const float CALL_TIMEOUT = 10f;
        
        private readonly ICoroutinePerformer _coroutinePerformer;
        
        private readonly Dictionary<string, LeaderBoardData> _cashedLeaderboard;
        private readonly Queue<LeaderboardCall> _callLeaderboards;
        private readonly Queue<Action> _saveLeaderboardCalls;
        private readonly Dictionary<string, float> _queueCallsTimeData;
        
        private int _lastCooldownCalls;

        private float _lastLeaderBoardCall;
        private float _lastLeaderBoardSaveCall;

        public YandexCloudLeaderboardProvider(ICoroutinePerformer coroutinePerformer)
        {
            _coroutinePerformer = coroutinePerformer;
            _cashedLeaderboard = new Dictionary<string, LeaderBoardData>();
            _callLeaderboards = new Queue<LeaderboardCall>();
            _queueCallsTimeData = new Dictionary<string, float>();
            _saveLeaderboardCalls = new Queue<Action>();

            YandexGame.onGetLeaderboard += OnLeaderBoardLoaded;
        }

        public void Dispose()
        {
            YandexGame.onGetLeaderboard -= OnLeaderBoardLoaded;
        }
        
        public IEnumerator GetCoinsLeaderBoard(Action<ILeaderBoardData> callback)
            => GetLeaderBoard(COINS_LEADERBOARD_KEY, callback);

        public IEnumerator GetLevelLeaderBoard(string levelID, Action<ILeaderBoardData> callback)
            => GetLeaderBoard(GetLeaderboardId(levelID), callback);
        
        public IEnumerator GetCurrentPlayerID(Action<string> callback)
        {
            callback?.Invoke(YandexGame.playerId);
            yield break;
        }

        public void SendCoinsSaveRequest(int coins)
            => _saveLeaderboardCalls.Enqueue(() => YandexGame.NewLeaderboardScores(COINS_LEADERBOARD_KEY, coins));

        public void SendLevelSaveRequest(string levelID, int milliSeconds)
        {
            _coroutinePerformer.StartCoroutine(SaveThisPlayerLocal(levelID, milliSeconds));
                
            _saveLeaderboardCalls.Enqueue(() => YandexGame.NewLBScoreTimeConvert(GetLeaderboardId(levelID), milliSeconds));

            CallLeaderboard(levelID);
                
            _cashedLeaderboard[GetLeaderboardId(levelID)].Sort();
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
            
            if (Time.time - _lastLeaderBoardCall >= LEADERBOARD_CALL_COOLDOWN)
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
        
        private IEnumerator SaveThisPlayerLocal(string key, int score)
        {
            if (!_cashedLeaderboard.ContainsKey(key))
            {
                LBData lBData = new LBData();
                lBData.technoName = GetLeaderboardId(key);
                lBData.thisPlayer = new LBThisPlayerData();
                LeaderBoardData data = new LeaderBoardData(lBData, _coroutinePerformer);
                data.Initialize();
                yield return new WaitUntil(() => data.Initialized);
                _cashedLeaderboard.Add(key, data);
            }

            bool found = false;

            foreach (var data in _cashedLeaderboard[key].GetEntries())
            {
                if (data.ID == YandexGame.playerId)
                {
                    data.Score = score;
                    found = true;
                }
            }

            if (!found)
            {
                LBPlayerData newData = new LBPlayerData();
                newData.uniqueID = YandexGame.playerId;
                newData.name = YandexGame.playerName;
                newData.photo = YandexGame.playerPhoto;
                newData.score = score;
                

                yield return _cashedLeaderboard[key].AddEntry(newData);
            }

            _cashedLeaderboard[key].UpdateScore(YandexGame.playerId, score);
        }

        private void CallLeaderboard(string key)
        {
            if (_queueCallsTimeData.ContainsKey(key))
                return;

            LeaderboardCall leaderboardCall = new LeaderboardCall()
            {
                call = () => YandexGame.GetLeaderboard(key, 10, 3, 3, "small"),
                leaderBoardId = GetLeaderboardId(key)
            };

            _callLeaderboards.Enqueue(leaderboardCall);
            _queueCallsTimeData.Add(GetLeaderboardId(key), Time.time);
        }

        private void OnLeaderBoardLoaded(LBData lBData)
        {
            LeaderBoardData leaderBoardData = new LeaderBoardData(lBData, _coroutinePerformer);
            leaderBoardData.Initialize();
            _cashedLeaderboard[lBData.technoName] = leaderBoardData;
        }

        private IEnumerator GetLeaderBoard(string key, Action<ILeaderBoardData> callback)
        {
            if (!_cashedLeaderboard.ContainsKey(key))
                CallLeaderboard(key);

            while
            (
                !_cashedLeaderboard.ContainsKey(key) &&
                _queueCallsTimeData.ContainsKey(key) &&
                Time.time - _queueCallsTimeData[key] < CALL_TIMEOUT
            )
                yield return new WaitForSeconds(1f);

            if (!_cashedLeaderboard.ContainsKey(key))
            {
                Debug.LogError($"Can't load leaderboard: {key}. Timeout!");
                callback?.Invoke(null);
                yield break;
            }

            callback?.Invoke(_cashedLeaderboard[key]);
        }
        
        private string GetLeaderboardId(string levelId) => LEADERBOARD_KEY + levelId;
        
        private struct LeaderboardCall
        {
            public string leaderBoardId;
            public Action call;
        }

        private class LeaderBoardData : ILeaderBoardData
        {
            private readonly Dictionary<string, LeaderBoardDataEntry> _entries =
                new Dictionary<string, LeaderBoardDataEntry>();
            
            private List<LeaderBoardDataEntry> _sortedEntries = new List<LeaderBoardDataEntry>();

            private readonly LBData _origin;
            private readonly ICoroutinePerformer _coroutinePerformer;
            
            public bool Initialized { get; private set; }

            public LeaderBoardData(LBData lbData, ICoroutinePerformer coroutinePerformer)
            {
                _origin = lbData;
                _coroutinePerformer = coroutinePerformer;
            }

            public void Initialize() => _coroutinePerformer.StartCoroutine(InitializeInternal());
            
            public bool TryGetEntryFor(string id, out LeaderBoardDataEntry entry)
                => _entries.TryGetValue(id, out entry);

            public IReadOnlyList<LeaderBoardDataEntry> GetEntries()
                => _entries.Values.ToList();

            public IEnumerator AddEntry(LBPlayerData playerData)
            {
                void OnConverted(LeaderBoardDataEntry entry)
                {
                    _entries[playerData.uniqueID] = entry;
                }
                    
                yield return LBPlayerDataToEntry(playerData, OnConverted);
            }

            public void UpdateScore(string key, int score)
            {
                if (_entries.ContainsKey(key))
                    _entries[key].Score = score;
            }

            public void Sort()
            {
                int countOfPlayersInLB = _sortedEntries.Count;
                int[] ranksBeforeSort = new int[countOfPlayersInLB];

                for (int i = 0; i < countOfPlayersInLB; i++)
                    ranksBeforeSort[i] = _sortedEntries[i].Rank;

                var sorted 
                    = _sortedEntries.OrderByDescending(player => player.Score);
                
                _sortedEntries.Clear();
                _sortedEntries.AddRange(sorted);

                for (int i = 0; i < countOfPlayersInLB; i++)
                {
                    _sortedEntries[i].Rank = ranksBeforeSort[i];
                }
            }

            private IEnumerator InitializeInternal()
            {
                _sortedEntries.Clear();
                Initialized = true;

                foreach (var lbPlayerData in _origin.players)
                {
                    void OnConverted(LeaderBoardDataEntry entry)
                    {
                        _entries[lbPlayerData.uniqueID] = entry;
                    }
                    
                    yield return LBPlayerDataToEntry(lbPlayerData, OnConverted);
                }
                
                _sortedEntries.AddRange(_entries.Values);
            }

            private IEnumerator LBPlayerDataToEntry(LBPlayerData playerData, Action<LeaderBoardDataEntry> callback)
            {
                SpriteURLLoader spriteURLLoader = new SpriteURLLoader();
                yield return spriteURLLoader.Load(playerData.photo);
                    
                LeaderBoardDataEntry entry = new LeaderBoardDataEntry()
                {
                    ID = playerData.uniqueID,
                    Name = playerData.name,
                    Photo = spriteURLLoader.LoadedSprite.Value,
                    Score = playerData.score,
                    Rank = playerData.rank,
                };
                
                callback?.Invoke(entry);
            }
        }
    }
}
