using System.Collections.Generic;
using Common.Reactive;

namespace Services.PlayerData.Core.Levels
{
    public interface ILevelsService
    {
        public IReadonlyObservable<string> SelectedLevel { get; }
        public IEnumerable<string> PassedLevels { get; }
        public bool IsLevelPassed(string levelID);
        public void AddPassedLevel(string levelID);
        public void SetSelectedLevel(string levelID);
    }
}