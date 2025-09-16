using System.Collections.Generic;

namespace Services.LeaderBoards
{
    public interface ILeaderBoardData
    {
        public bool TryGetEntryFor(string id, out LeaderBoardDataEntry entry);
        public IReadOnlyList<LeaderBoardDataEntry> GetEntries();

    }
}
