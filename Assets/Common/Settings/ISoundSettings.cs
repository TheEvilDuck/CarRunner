using System;

namespace Common
{
    public interface ISoundSettings : ISettings
    {
        public bool Muted { get; }
        public float MasterVolume { get; }
        public float BackgroundMusicVolume { get; }
        public float SFXSoundVolume { get; }
        public event Action SoundSettingsChanged;

        public void SoundOn(bool flag);
        public void SetMasterVolume(float volume);
        public void SetBackgroundMusicVolume(float volume);
        public void SetSFXSoundsVolume(float volume);
    }
}