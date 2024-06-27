using System;
using UnityEngine;

namespace Common
{
    public class GameSettings
    {
        private const string GAME_SETTINGS_KEY = "GameSettingsKey";
        private SoundSettings _settings;
        public Action SoundSettingsChanged;

        public bool IsSoundOn => _settings.IsSoundOn;
        public float MasterVolume => _settings.MasterVolume;
        public float BackgroundMusicVolume => _settings.BackgroundMusicVolume;
        public float SFXSoundVolume => _settings.SFXSoundVolume;

        public GameSettings()
        {
            _settings = new SoundSettings();
        }

        public void SoundOn(bool flag)
        {
            _settings.IsSoundOn = flag;
            SoundSettingsChanged?.Invoke();
        }
        public void SetMasterVolume(float volume)
        {
            _settings.MasterVolume = volume;
            SoundSettingsChanged?.Invoke();
        }
        public void SetBackgroundMusicVolume(float volume)
        {
            _settings.BackgroundMusicVolume = volume;
            SoundSettingsChanged?.Invoke();
        }
        public void SetSFXSoundsVolume(float volume)
        {
            _settings.SFXSoundVolume = volume;
            SoundSettingsChanged?.Invoke();
        }

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
                _settings = JsonUtility.FromJson<SoundSettings>(gameSettings);
            }
            else
                throw new Exception("Settings not found");
        }

        [Serializable]
        private class SoundSettings
        {
            public bool IsSoundOn;
            public float MasterVolume;
            public float BackgroundMusicVolume;
            public float SFXSoundVolume;
        }
    }
}