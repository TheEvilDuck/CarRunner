using System;
using UnityEngine;

namespace Common
{
    public class GameSettings : ICameraSettings, ISoundSettings
    {
        private const string GAME_SOUND_SETTINGS_KEY = "GameSoundSettingsKey";
        private const string GAME_CAMERA_SETTINGS_KEY = "GameCameraSettingsKey";
        public event Action SoundSettingsChanged;
        public event Action CameraSettingsChanged; 
        private SoundSettings _soundSettings;
        private CameraSettings _cameraSettings;

        public bool Muted => _soundSettings.Mute;
        public float MasterVolume => _soundSettings.MasterVolume;
        public float BackgroundMusicVolume => _soundSettings.BackgroundMusicVolume;
        public float SFXSoundVolume => _soundSettings.SFXSoundVolume;
        public float AngleOfView => _cameraSettings.AngleOfView;
        public float ZOffset => _cameraSettings.ZOffset;

        public GameSettings() => LoadSettings();

        public void SoundOn(bool flag)
        {
            _soundSettings.Mute = flag;
            SoundSettingsChanged?.Invoke();
        }

        public void SetMasterVolume(float volume)
        {
            _soundSettings.MasterVolume = volume;
            SoundSettingsChanged?.Invoke();
        }

        public void SetBackgroundMusicVolume(float volume)
        {
            _soundSettings.BackgroundMusicVolume = volume;
            SoundSettingsChanged?.Invoke();
        }
        
        public void SetSFXSoundsVolume(float volume)
        {
            _soundSettings.SFXSoundVolume = volume;
            SoundSettingsChanged?.Invoke();
        }

        public void SetAngleOfView(float value)
        {
            _cameraSettings.AngleOfView = value;
            CameraSettingsChanged?.Invoke();
        }

        public void SetZOffset(float value)
        {
            _cameraSettings.ZOffset = value;
            CameraSettingsChanged?.Invoke();
        }

        public void SaveSettings()
        {
            string soundSettings = JsonUtility.ToJson(_soundSettings);
            string cameraSettings = JsonUtility.ToJson(_cameraSettings);
            PlayerPrefs.SetString(GAME_SOUND_SETTINGS_KEY, soundSettings);
            PlayerPrefs.SetString(GAME_CAMERA_SETTINGS_KEY, cameraSettings);
        }

        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey(GAME_SOUND_SETTINGS_KEY) && PlayerPrefs.HasKey(GAME_CAMERA_SETTINGS_KEY))
            {
                string soundSettingJson = PlayerPrefs.GetString(GAME_SOUND_SETTINGS_KEY);
                string cameraSettingsJson = PlayerPrefs.GetString(GAME_CAMERA_SETTINGS_KEY);
                _soundSettings = JsonUtility.FromJson<SoundSettings>(soundSettingJson);
                _cameraSettings = JsonUtility.FromJson<CameraSettings>(cameraSettingsJson);
            }
            else 
            {
                _soundSettings = new SoundSettings();
                _cameraSettings = new CameraSettings();
                SaveSettings();
            }

            SoundSettingsChanged?.Invoke();
            CameraSettingsChanged?.Invoke();
        }

        //All filds are stored in a normalized form 
        [Serializable]
        private class SoundSettings
        {
            public bool Mute = true;
            public float MasterVolume = 0.5f;
            public float BackgroundMusicVolume = 0.5f;
            public float SFXSoundVolume = 0.5f;
        }

        [Serializable]
        private class CameraSettings
        {
            public float AngleOfView = 0.5f;
            public float ZOffset = 0.5f;
        }
    }
}