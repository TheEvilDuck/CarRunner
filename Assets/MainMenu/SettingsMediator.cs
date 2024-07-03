using Common;
using MainMenu;
using System;

public class SettingsMediator : IDisposable
{
    private GameSettings _settings;
    private SettingsMenu _menu;

    public SettingsMediator(GameSettings settings, SettingsMenu menu)
    {
        _settings = settings;
        _menu = menu;

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
