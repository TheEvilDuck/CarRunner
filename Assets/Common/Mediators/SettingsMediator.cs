using Common;
using Common.UI;
using DI;
using System;

public class SettingsMediator : IDisposable
{
    private readonly GameSettings _settings;
    private readonly SoundSettingsView _soundSettingsView;
    private readonly CameraSettingsView _cameraSettingsView;

    public SettingsMediator(DIContainer sceneContext)
    {
        _settings = sceneContext.Get<GameSettings>();
        _soundSettingsView = sceneContext.Get<SettingsMenu>().SoundSettingsView;
        _cameraSettingsView = sceneContext.Get<SettingsMenu>().CameraSettingsView;

        _soundSettingsView.MuteChanged.AddListener(onMuteChanged);
        _soundSettingsView.MasterVolumeChanged.AddListener(onMasterVolumeChanged);
        _soundSettingsView.BackgroundMusicVolumeChanged.AddListener(onBackgroundMusicVolumeChanged);
        _soundSettingsView.SFXSoundVolumeChanged.AddListener(onSFXSoundVolumeChanged);
    }

    private void onMuteChanged(bool mute) => _settings.SoundOn(mute);
    private void onMasterVolumeChanged(float volume) => _settings.SetMasterVolume(volume);
    private void onBackgroundMusicVolumeChanged(float volume) => _settings.SetBackgroundMusicVolume(volume);
    private void onSFXSoundVolumeChanged(float volume) => _settings.SetSFXSoundsVolume(volume);

    public void Dispose()
    {
        _soundSettingsView.MuteChanged.RemoveListener(onMuteChanged);
        _soundSettingsView.MasterVolumeChanged.RemoveListener(onMasterVolumeChanged);
        _soundSettingsView.BackgroundMusicVolumeChanged.RemoveListener(onBackgroundMusicVolumeChanged);
        _soundSettingsView.SFXSoundVolumeChanged.RemoveListener(onSFXSoundVolumeChanged);
        _settings.SaveSettings();
    }
}
