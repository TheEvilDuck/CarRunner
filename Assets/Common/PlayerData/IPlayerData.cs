using System.Collections.Generic;

namespace Common.Data
{
    public interface IPlayerData
    {
        public IEnumerable<string> AvailableLevels {get;}
        public IEnumerable<string> PassedLevels {get;}
        public string SelectedLevel {get;}
        public void SaveSelectedLevel(string levelId);
        public void AddPassedLevel(string levelId);
        public void AddAvailableLevel(string levelId);
        public bool LoadProgressOfLevels();
    }

}