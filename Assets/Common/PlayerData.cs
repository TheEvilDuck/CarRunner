using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class PlayerData
    {
        private const string PREFS_SELECTED_LEVEL = "PLAYERPREFS_SELECTED_LEVEL";
        private const string PREFS_PROGRESS_OF_LEVELS = "PLAYERPREFS_PROGRESS_OF_LEVELS";

        private ProgressOfLevels _progressOfLvls;
        
        public IEnumerable<string> AvailableLevels => _progressOfLvls.AvailableLevels;
        public IEnumerable<string> PassedLevels => _progressOfLvls.PassedLevels;

        public string SelectedLevel => PlayerPrefs.GetString(PREFS_SELECTED_LEVEL);
        public void SaveSelectedLevel(string levelId) => PlayerPrefs.SetString(PREFS_SELECTED_LEVEL, levelId);

        public void AddPassedLevel(string levelId)
        {
            if (_progressOfLvls.PassedLevels.Contains(levelId))
                return;
            else
            {
                _progressOfLvls.PassedLevels.Add(levelId);
                SaveProgressOfLevels();
            }
        }

        public void AddAvailableLevel(string levelId)
        {
            if (_progressOfLvls.AvailableLevels.Contains(levelId))
                return;
            else
            {
                _progressOfLvls.AvailableLevels.Add(levelId);
                SaveProgressOfLevels();
            }
        }

        private void SaveProgressOfLevels()
        {
            string progressOfLevels = JsonUtility.ToJson(_progressOfLvls);
            PlayerPrefs.SetString(PREFS_PROGRESS_OF_LEVELS, progressOfLevels);
        }

        public bool LoadProgressOfLevels()
        {
            string progressOfLevels;
            if (PlayerPrefs.HasKey(PREFS_PROGRESS_OF_LEVELS))
            {
                progressOfLevels = PlayerPrefs.GetString(PREFS_PROGRESS_OF_LEVELS);
                _progressOfLvls = JsonUtility.FromJson<ProgressOfLevels>(progressOfLevels);
                return true;
            }
            else
            {
                _progressOfLvls = new ProgressOfLevels();
                return false;
            }
        }

        [Serializable]
        private class ProgressOfLevels
        {
            public List<string> PassedLevels;
            public List<string> AvailableLevels;

            public ProgressOfLevels()
            {
                PassedLevels = new List<string>();
                AvailableLevels = new List<string>();
            }
        }
    }
}
