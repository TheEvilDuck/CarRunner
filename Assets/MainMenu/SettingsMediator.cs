using Common;
using DI;
using MainMenu;
using System;

public class SettingsMediator : IDisposable
{
    private readonly GameSettings _settings;
    private readonly SettingsMenu _menu;

    public SettingsMediator(DIContainer sceneContext)
    {
        _settings = sceneContext.Get<GameSettings>();
        _menu = sceneContext.Get<SettingsMenu>();

        _menu.MuteChanged.AddListener(onMuteChanged);
        _menu.MasterVolumeChanged.AddListener(onMasterVolumeChanged);
        _menu.BackgroundMusicVolumeChanged.AddListener(onBackgroundMusicVolumeChanged);
        _menu.SFXSoundVolumeChanged.AddListener(onSFXSoundVolumeChanged);
    }

    private void onMuteChanged(bool mute) => _settings.SoundOn(mute);
    private void onMasterVolumeChanged(float volume) => _settings.SetMasterVolume(volume);
    private void onBackgroundMusicVolumeChanged(float volume) => _settings.SetBackgroundMusicVolume(volume);
    private void onSFXSoundVolumeChanged(float volume) => _settings.SetSFXSoundsVolume(volume);

    public void Dispose()
    {
        _menu.MuteChanged.RemoveListener(onMuteChanged);
        _menu.MasterVolumeChanged.RemoveListener(onMasterVolumeChanged);
        _menu.BackgroundMusicVolumeChanged.RemoveListener(onBackgroundMusicVolumeChanged);
        _menu.SFXSoundVolumeChanged.RemoveListener(onSFXSoundVolumeChanged);
        _settings.SaveSettings();
    }
}
