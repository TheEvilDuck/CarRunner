using System;

public class SettingsMediator : IDisposable
{
    private GameSettings _settings;
    private SettingsMenu _menu;

    public SettingsMediator(GameSettings settings, SettingsMenu menu)
    {
        _settings = settings;
        _menu = menu;

        _menu.IsMusicOnChanged.AddListener(SoundOn);
        _menu.MasterVolumeChenged.AddListener(SetMasterVolume);
        _menu.BackgroundMusicVolumeChanged.AddListener(SetBackgroundMusicVolume);
        _menu.SFXSoundVolumeChanged.AddListener(SetSFXSoundsVolume);
    }

    public void SoundOn(bool flag) => _settings.SoundOn(flag);
    public void SetMasterVolume(float volume) => _settings.SetMasterVolume(volume);
    public void SetBackgroundMusicVolume(float volume) => _settings.SetBackgroundMusicVolume(volume);
    public void SetSFXSoundsVolume(float volume) => _settings.SetSFXSoundsVolume(volume);

    public void Dispose()
    {
        _menu.IsMusicOnChanged.RemoveListener(SoundOn);
        _menu.MasterVolumeChenged.RemoveListener(SetMasterVolume);
        _menu.BackgroundMusicVolumeChanged.RemoveListener(SetBackgroundMusicVolume);
        _menu.SFXSoundVolumeChanged.RemoveListener(SetSFXSoundsVolume);
        _settings.SaveSettings();
    }
}
