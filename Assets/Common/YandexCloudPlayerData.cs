using Common;
using System.Collections.Generic;
using YG;

public class YandexCloudPlayerData : IPlayerData
{
    private List<string> _availableLevels = YandexGame.savesData.AvailableLevels;
    private List<string> _passedLevels = YandexGame.savesData.PassedLevels;

    public IEnumerable<string> AvailableLevels => _availableLevels;
    public IEnumerable<string> PassedLevels => _passedLevels;
    public string SelectedLevel => YandexGame.savesData.SelectedLevel;

    public void AddAvailableLevel(string levelId)
    {
        if (_availableLevels.Contains(levelId))
            return;
        else
        {
            _availableLevels.Add(levelId);
            YandexGame.SaveProgress();
        }
    }

    public void AddPassedLevel(string levelId)
    {
        if(_passedLevels.Contains(levelId))
            return;
        else
        {
            _passedLevels.Add(levelId);
            YandexGame.SaveProgress();
        }
    }

    public bool LoadProgressOfLevels() => YandexGame.SDKEnabled;

    public void SaveSelectedLevel(string levelId)
    {
        YandexGame.savesData.SelectedLevel = levelId;
        YandexGame.SaveProgress();
    }
}