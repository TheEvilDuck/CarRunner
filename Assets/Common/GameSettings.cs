using System;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField]private Settings _settings;

    public void SoundOn(bool flag) => _settings.IsSoundOn = flag;
    public void SetMasterVolume(float volume) => _settings.MasterVolume = volume;
    public void SetBackgroundMusicVolume(float volume) => _settings.BackgroundMusicVolume = volume;
    public void SetSFXSoundsVolume(float volume) => _settings.SFXSoundVolume = volume;

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(_settings);
        PlayerPrefs.SetString("json", json);
    }

    public void LoadSettings()
    {
        string json;
        if (PlayerPrefs.HasKey("json"))
        {
            json = PlayerPrefs.GetString("json");
            _settings = JsonUtility.FromJson<Settings>(json);
        }
        else
            throw new Exception("Settings now found");
    }

    [Serializable]
    private class Settings
    {
        public bool IsSoundOn;
        public float MasterVolume;
        public float BackgroundMusicVolume;
        public float SFXSoundVolume;
    }
}