using System;
using UnityEngine;
using YG.Utils.LB;

namespace Common.Data
{
    public interface ILeaderBoardData
    {
        public event Action leaderboardUpdated;
        public Awaitable SaveLevelRecord(string levelId, float recordTime);
        public Awaitable<float> GetLevelRecord(string levelId);
        public Awaitable<LBData> GetLeaderBoard(string levelId);

    }
}
