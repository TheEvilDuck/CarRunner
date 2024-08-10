using System;
using Common.Sound;
using DI;

namespace Common.Mediators
{
    public class SettingsAndSoundMediator: IDisposable
    {
        private readonly SoundController _soundController;
        private readonly ISoundSettings _soundSettings;

        public SettingsAndSoundMediator(DIContainer sceneContext)
        {
            _soundSettings = sceneContext.Get<ISoundSettings>();
            _soundController = sceneContext.Get<SoundController>();

            UpdateSoundSettings();
            _soundSettings.SoundSettingsChanged += UpdateSoundSettings;
        }

        public void Dispose()
        {
            _soundSettings.SoundSettingsChanged -= UpdateSoundSettings;
        }

        private void UpdateSoundSettings()
        {
            if (!_soundSettings.Muted)
            {
                _soundController.SetValue(AudioMixerExposedParameters.VolumeMaster, _soundSettings.MasterVolume);
                _soundController.SetValue(AudioMixerExposedParameters.VolumeBackgroundMusic, _soundSettings.BackgroundMusicVolume);
                _soundController.SetValue(AudioMixerExposedParameters.VolumeSFX, _soundSettings.SFXSoundVolume);
            }
            else
                _soundController.SoundOff();
        }
    }

}