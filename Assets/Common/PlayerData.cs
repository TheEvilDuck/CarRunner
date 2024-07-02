using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class PlayerData
    {
        public ProgressOfLevels ProgressOfLvls = new ProgressOfLevels();

        private const string PREFS_SELECTED_LEVEL = "PLAYERPREFS_SELECTED_LEVEL";
        private const string PREFS_PROGRESS_OF_LEVELS = "PLAYERPREFS_PROGRESS_OF_LEVELS";
        public string SelectedLevel => PlayerPrefs.GetString(PREFS_SELECTED_LEVEL);
        public void SaveSelectedLevel(string levelId) => PlayerPrefs.SetString(PREFS_SELECTED_LEVEL, levelId);

        public void AddPassedLevel(string levelId)
        {
            if (ProgressOfLvls.PassedLevels.Contains(levelId))
                return;
            else
            {
                ProgressOfLvls.PassedLevels.Add(levelId);
                SaveProgressOfLevels();
            }
        }

        public void AddAvailableLevel(string levelId)
        {
            if (ProgressOfLvls.AvailableLevels.Contains(levelId))
                return;
            else
            {
                ProgressOfLvls.AvailableLevels.Add(levelId);
                SaveProgressOfLevels();
            }
        }

        private void SaveProgressOfLevels()
        {
            string progressOfLevels = JsonUtility.ToJson(ProgressOfLvls);
            PlayerPrefs.SetString(PREFS_PROGRESS_OF_LEVELS, progressOfLevels);
        }

        public void LoadProgressOfLevels()
        {
            string progressOfLevels;
            if (PlayerPrefs.HasKey(PREFS_PROGRESS_OF_LEVELS))
            {
                progressOfLevels = PlayerPrefs.GetString(PREFS_PROGRESS_OF_LEVELS);
                ProgressOfLvls = JsonUtility.FromJson<ProgressOfLevels>(progressOfLevels);
            }
        }

        [Serializable]
        public class ProgressOfLevels
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
