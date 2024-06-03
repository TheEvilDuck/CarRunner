using UnityEngine;

namespace Common
{
    public class PlayerData
    {
        private const string PREFS_SELECTED_LEVEL = "PLAYERPREFS_SELECTED_LEVEL";
        public string SelectedLevel => PlayerPrefs.GetString(PREFS_SELECTED_LEVEL);
        public void SaveSelectedLevel(string levelId) => PlayerPrefs.SetString(PREFS_SELECTED_LEVEL, levelId);
    }
}
