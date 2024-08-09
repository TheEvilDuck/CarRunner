using Common.UI;
using DI;
using System;

namespace Common.Mediators
{
    public class SettingsAndUIMediator : IDisposable
    {
        private readonly GameSettings _settings;
        private readonly SoundSettingsView _soundSettingsView;
        private readonly CameraSettingsView _cameraSettingsView;

        public SettingsAndUIMediator(DIContainer sceneContext)
        {
            _settings = sceneContext.Get<GameSettings>();
            _soundSettingsView = sceneContext.Get<GameSettingsUI>().SoundSettingsView;
            _cameraSettingsView = sceneContext.Get<GameSettingsUI>().CameraSettingsView;

            _soundSettingsView.MuteChanged.AddListener(onMuteChanged);
            _soundSettingsView.MasterVolumeChanged.AddListener(onMasterVolumeChanged);
            _soundSettingsView.BackgroundMusicVolumeChanged.AddListener(onBackgroundMusicVolumeChanged);
            _soundSettingsView.SFXSoundVolumeChanged.AddListener(onSFXSoundVolumeChanged);
            _cameraSettingsView.AngleOfViewChanged.AddListener(onAngleOfViewChanged);
            _cameraSettingsView.ZOffsetChanged.AddListener(onZOffsetChange);
        }

        private void onMuteChanged(bool mute) => _settings.SoundOn(mute);
        private void onMasterVolumeChanged(float volume) => _settings.SetMasterVolume(volume);
        private void onBackgroundMusicVolumeChanged(float volume) => _settings.SetBackgroundMusicVolume(volume);
        private void onSFXSoundVolumeChanged(float volume) => _settings.SetSFXSoundsVolume(volume);
        private void onAngleOfViewChanged(float value) => _settings.SetAngleOfView(value);
        private void onZOffsetChange(float value) => _settings.SetZOffset(value);

        public void Dispose()
        {
            _soundSettingsView.MuteChanged.RemoveListener(onMuteChanged);
            _soundSettingsView.MasterVolumeChanged.RemoveListener(onMasterVolumeChanged);
            _soundSettingsView.BackgroundMusicVolumeChanged.RemoveListener(onBackgroundMusicVolumeChanged);
            _soundSettingsView.SFXSoundVolumeChanged.RemoveListener(onSFXSoundVolumeChanged);
            _cameraSettingsView.AngleOfViewChanged.RemoveListener(onAngleOfViewChanged);
            _cameraSettingsView.ZOffsetChanged.RemoveListener(onZOffsetChange);
            _settings.SaveSettings();
        }
    }
}