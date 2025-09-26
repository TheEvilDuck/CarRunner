using System.Collections.Generic;

namespace Services.PlayerData.Core.Levels
{
    public interface ILevelsData: IData
    {
        public string SelectedLevel { get; }
        public IEnumerable<string> PassedLevels { get; }
    }
}