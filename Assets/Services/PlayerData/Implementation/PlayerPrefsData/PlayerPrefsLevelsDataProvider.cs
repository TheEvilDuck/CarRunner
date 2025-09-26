using System;
using System.Collections;
using System.Collections.Generic;
using Common.CoroutineCallbacks;
using Services.PlayerData.Core;
using Services.PlayerData.Core.Levels;
using UnityEngine;

namespace Services.PlayerData.Implementation.PlayerPrefsData
{
    public class PlayerPrefsLevelsDataProvider: IDataProvider<ILevelsData>
    {
        private const string SELECTED_LEVEL_KEY = "PLAYERPREFS_SELECTED_LEVEL";
        private const string PROGRESS_OF_LEVELS_KEY = "PLAYERPREFS_PROGRESS_OF_LEVELS";
        
        public IEnumerator Load(DataLoadCallback<ILevelsData> callback)
        {
            if (!PlayerPrefs.HasKey(SELECTED_LEVEL_KEY) || !PlayerPrefs.HasKey(PROGRESS_OF_LEVELS_KEY))
            {
                callback?.Invoke(false, null);
                yield break;
            }
            
            string selectedLevelId = PlayerPrefs.GetString(SELECTED_LEVEL_KEY);
            string progressOfLevelsJson = PlayerPrefs.GetString(PROGRESS_OF_LEVELS_KEY);
            
            ProgressOfLevels progressOfLevels = JsonUtility.FromJson<ProgressOfLevels>(progressOfLevelsJson);

            LevelsData levelsData = new LevelsData(selectedLevelId, progressOfLevels.PassedLevels);
            callback?.Invoke(true, levelsData);
            yield break;
        }

        public IEnumerator Save(ILevelsData data, SuccessCallback callback)
        {
            ProgressOfLevels progressOfLevels = new ProgressOfLevels();
            progressOfLevels.PassedLevels.AddRange(data.PassedLevels);
            
            string progressOfLevelsJson = JsonUtility.ToJson(progressOfLevels);
            PlayerPrefs.SetString(PROGRESS_OF_LEVELS_KEY, progressOfLevelsJson);
            
            PlayerPrefs.SetString(SELECTED_LEVEL_KEY, data.SelectedLevel);
            
            callback?.Invoke(true);
            yield break;
        }
        
        [Serializable]
        private class ProgressOfLevels
        {
            public List<string> PassedLevels = new List<string>();
        }

        private class LevelsData : ILevelsData
        {
            public string SelectedLevel { get; }
            public IEnumerable<string> PassedLevels { get; }
            
            public LevelsData(string selectedLevel, IEnumerable<string> passedLevels)
            {
                SelectedLevel = selectedLevel;
                PassedLevels = passedLevels;
            }
        }
    }
}