using System;
using UnityEngine;

namespace Common
{
    public class GameSettings
    {
        private const string GAME_SETTINGS_KEY = "GameSettingsKey";
        private Settings _settings;

        public bool IsSoundOn => _settings.IsSoundOn;
        public float MasterVolume => _settings.MasterVolume;
        public float BackgroundMusicVolume => _settings.BackgroundMusicVolume;
        public float SFXSoundVolume => _settings.SFXSoundVolume;

        public GameSettings()
        {
            _settings = new Settings();
        }

        public void SoundOn(bool flag) => _settings.IsSoundOn = flag;
        public void SetMasterVolume(float volume) => _settings.MasterVolume = volume;
        public void SetBackgroundMusicVolume(float volume) => _settings.BackgroundMusicVolume = volume;
        public void SetSFXSoundsVolume(float volume) => _settings.SFXSoundVolume = volume;

        public void SaveSettings()
        {
            string gameSettings = JsonUtility.ToJson(_settings);
            PlayerPrefs.SetString(GAME_SETTINGS_KEY, gameSettings);
        }

        public void LoadSettings()
        {
            string gameSettings;
            if (PlayerPrefs.HasKey(GAME_SETTINGS_KEY))
            {
                gameSettings = PlayerPrefs.GetString(GAME_SETTINGS_KEY);
                _settings = JsonUtility.FromJson<Settings>(gameSettings);
            }
            else
                throw new Exception("Settings not found");
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
}