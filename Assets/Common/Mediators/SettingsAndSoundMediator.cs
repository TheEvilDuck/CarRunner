using System;
using Common.Sound;

namespace Common.Mediators
{
    public class SettingsAndSoundMediator: IDisposable
    {
        private readonly SoundController _soundController;
        private readonly GameSettings _gameSettings;

        public SettingsAndSoundMediator(GameSettings gameSettings, SoundController soundController)
        {
            _gameSettings = gameSettings;
            _soundController = soundController;

            UpdateSoundSettings();
            _gameSettings.SoundSettingsChanged += UpdateSoundSettings;
        }

        public void Dispose()
        {
            _gameSettings.SoundSettingsChanged -= UpdateSoundSettings;
        }

        private void UpdateSoundSettings()
        {
            if (!_gameSettings.Muted)
            {
                _soundController.SetValue(AudioMixerExposedParameters.VolumeMaster, _gameSettings.MasterVolume);
                _soundController.SetValue(AudioMixerExposedParameters.VolumeBackgroundMusic, _gameSettings.BackgroundMusicVolume);
                _soundController.SetValue(AudioMixerExposedParameters.VolumeSFX, _gameSettings.SFXSoundVolume);
            }
            else
                _soundController.SoundOff();
        }
    }

}