using System;
using UnityEngine;
using YG.Utils.LB;

namespace Common.Data
{
    public interface ILeaderBoardData
    {
        public event Action<LBData> leaderboardUpdated;
        public Awaitable SaveLevelRecord(string levelId, float recordTime);
        public void SaveCoins(int coins);
        public Awaitable<float> GetLevelRecord(string levelId);
        public Awaitable<LBData> GetLeaderBoard(string levelId);
        public string GetLeaderboardId(string levelId);

    }
}
