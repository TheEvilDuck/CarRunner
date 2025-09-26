using System.Collections.Generic;
using System.Linq;
using Levels.Scripts;

namespace Services.PlayerData.Core.Levels
{
    public class AvailableLevelsService
    {
        private readonly LevelsDatabase _levelsDatabase;
        private readonly ILevelsService _levelsService;

        public AvailableLevelsService(
            LevelsDatabase levelsDatabase, 
            ILevelsService levelsService)
        {
            _levelsDatabase = levelsDatabase;
            _levelsService = levelsService;
        }

        public bool IsLevelAvailable(string levelID)
        {
            if (string.Equals(_levelsDatabase.TutorialLevelId, levelID))
                return true;

            if (string.Equals(_levelsDatabase.GetFirstLevel(), levelID))
                return true;

            IEnumerable<string> availableLevels = GetAvailableLevels();

            return availableLevels.Contains(levelID);
        }

        public IEnumerable<string> GetAvailableLevels()
        {
            List<string> availableLevels = new List<string>();
            
            availableLevels.Add(_levelsDatabase.TutorialLevelId);
            availableLevels.Add(_levelsDatabase.GetFirstLevel());

            foreach (string levelID in _levelsDatabase.GetAllLevels())
            {
                if (availableLevels.Contains(levelID))
                    continue;

                if (_levelsService.PassedLevels.Contains(levelID) && !availableLevels.Contains(levelID))
                {
                    availableLevels.Add(levelID);

                    string nextLevelID = _levelsDatabase.GetNextLevelId(levelID);
                    
                    if (!availableLevels.Contains(nextLevelID))
                        availableLevels.Add(nextLevelID);
                }
            }

            return availableLevels;
        }
    }
}