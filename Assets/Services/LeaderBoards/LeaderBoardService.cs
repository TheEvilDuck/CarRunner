using System;
using System.Collections;
using System.Collections.Generic;
using Common.CoroutinePerformer;

namespace Services.LeaderBoards
{
    public class LeaderBoardService: ILeaderBoardService
    {
        private readonly ILeaderBoardProvider _leaderBoardProvider;

        public LeaderBoardService(ILeaderBoardProvider leaderBoardProvider)
        {
            _leaderBoardProvider = leaderBoardProvider;
        }

        public IEnumerator SaveLevelRecordAsync(string levelId, float recordTime)
        {
            void GetLevelCallback(bool success, float previousRecordTime)
            {
                if (success && recordTime <= previousRecordTime)
                    return;
                
                _leaderBoardProvider.SendLevelSaveRequest(levelId, ConvertSecondsToMilliseconds(recordTime));
            }
            
            yield return GetLevelRecordAsync(levelId, GetLevelCallback);
        }

        public IEnumerator SaveCoinsRecordAsync(int coins)
        {
            void GetCoinsCallback(bool success, int previousCoinsRecord)
            {
                if (success && coins <= previousCoinsRecord)
                    return;
                
                _leaderBoardProvider.SendCoinsSaveRequest(coins);
            }

            yield return GetCoinsRecordAsync(GetCoinsCallback);
        }

        public IEnumerator GetLevelRecordAsync(string levelId, Action<bool, float> onResult)
        {
            string currentPlayerID = null;

            void GetCurrentPlayerIDCallback(string playerId)
                => currentPlayerID = playerId;
            
            void GetLevelCallback(bool success, ILeaderBoardData data)
            {
                if (success == false || data == null)
                    onResult?.Invoke(false, 0);
                
                if (data.TryGetEntryFor(currentPlayerID, out LeaderBoardDataEntry entry) == false)
                    onResult?.Invoke(false, 0);
                
                onResult?.Invoke(true, ConvertMillisecondsToSeconds(entry.Score));
            }
            
            yield return _leaderBoardProvider.GetCurrentPlayerID(GetCurrentPlayerIDCallback);
            yield return GetLevelLeaderBoardAsync(levelId, GetLevelCallback);
        }

        public IEnumerator GetCoinsRecordAsync(Action<bool, int> onResult)
        {
            string currentPlayerID = null;

            void GetCurrentPlayerIDCallback(string playerId)
                => currentPlayerID = playerId;
            
            void GetCoinsCallback(bool success, ILeaderBoardData data)
            {
                if (success == false || data == null)
                {
                    onResult?.Invoke(false, 0);
                    return;
                }

                if (data.TryGetEntryFor(currentPlayerID, out LeaderBoardDataEntry entry) == false)
                {
                    onResult?.Invoke(false, 0);
                    return;
                }
                
                onResult?.Invoke(true, entry.Score);
            }

            yield return _leaderBoardProvider.GetCurrentPlayerID(GetCurrentPlayerIDCallback);
            yield return GetCoinsLeaderBoardAsync(GetCoinsCallback);
        }

        public IEnumerator GetLevelLeaderBoardAsync(string levelId, Action<bool, ILeaderBoardData> onResult)
        {
            void GetLevelLeaderBoardCallback(ILeaderBoardData data)
                => onResult?.Invoke(data != null, data);
            
            yield return _leaderBoardProvider.GetLevelLeaderBoard(levelId, GetLevelLeaderBoardCallback);
            
        }

        public IEnumerator GetCoinsLeaderBoardAsync(Action<bool, ILeaderBoardData> onResult)
        {
            void GetCoinsLeaderBoardCallback(ILeaderBoardData data)
                => onResult?.Invoke(data != null, data);

            yield return _leaderBoardProvider.GetCoinsLeaderBoard(GetCoinsLeaderBoardCallback);
        }

        private float ConvertMillisecondsToSeconds(int milliseconds) => milliseconds / 1000f;
        private int ConvertSecondsToMilliseconds(float seconds) => (int)(seconds * 1000f);
    }
}