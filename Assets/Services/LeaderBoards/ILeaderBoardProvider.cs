using System;
using System.Collections;

namespace Services.LeaderBoards
{
    public interface ILeaderBoardProvider
    {
        public IEnumerator GetCoinsLeaderBoard(Action<ILeaderBoardData> callback);
        public IEnumerator GetLevelLeaderBoard(string levelID, Action<ILeaderBoardData> callback);
        public IEnumerator GetCurrentPlayerID(Action<string> callback);

        public void SendCoinsSaveRequest(int coins);
        public void SendLevelSaveRequest(string levelID, int milliSeconds);
    }
}