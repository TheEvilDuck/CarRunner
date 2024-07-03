using System;
using UnityEngine;

namespace Common
{
    public class GameSettings
    {
        private const string GAME_SETTINGS_KEY = "GameSettingsKey";
        private SoundSettings _settings;
        public Action SoundSettingsChanged;

        public bool Muted => _settings.Mute;
        public float MasterVolume => _settings.MasterVolume;
        public float BackgroundMusicVolume => _settings.BackgroundMusicVolume;
        public float SFXSoundVolume => _settings.SFXSoundVolume;

        public void SoundOn(bool flag)
        {
            _settings.Mute = flag;
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
            if (PlayerPrefs.HasKey(GAME_SETTINGS_KEY))
            {
                string settingJson = PlayerPrefs.GetString(GAME_SETTINGS_KEY);
                _settings = JsonUtility.FromJson<SoundSettings>(settingJson);
            }
            else 
            {
                _settings = new SoundSettings();
                SaveSettings();
            }

            SoundSettingsChanged?.Invoke();
        }

        [Serializable]
        private class SoundSettings
        {
            public bool Mute = true;
            public float MasterVolume = 0.5f;
            public float BackgroundMusicVolume = 0.5f;
            public float SFXSoundVolume = 0.5f;
        }
    }
}