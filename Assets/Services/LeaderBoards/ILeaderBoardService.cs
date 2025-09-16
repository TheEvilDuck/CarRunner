using System;
using System.Collections;

namespace Services.LeaderBoards
{
    public interface ILeaderBoardService
    {
        public IEnumerator SaveLevelRecordAsync(string levelId, float recordTime);
        public IEnumerator SaveCoinsRecordAsync(int coins);
        public IEnumerator GetLevelRecordAsync(string levelId, Action<bool, float> onResult);
        public IEnumerator GetCoinsRecordAsync(Action<bool, int> onResult);
        public IEnumerator GetLevelLeaderBoardAsync(string levelId, Action<bool, ILeaderBoardData> onResult);
        public IEnumerator GetCoinsLeaderBoardAsync(Action<bool, ILeaderBoardData> onResult);
    }
}