using Common;
using Common.Sound;
using Common.States;
using Gameplay.Garages;
using Gameplay.TimerGates;
using System;

namespace Gameplay
{
    public class SoundMediator : IDisposable
    {
        private SoundController _soundController;
        private TimerGate[] _gates;
        private Garage[] _garages;
        private State _raceGameState;
        private GameSettings _gameSettings;

        public SoundMediator(SoundController soundController, TimerGate[] gates, Garage[] garages, State raceGameState, GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _soundController = soundController;
            _gates = gates;
            _garages = garages;
            _raceGameState = raceGameState;

            _raceGameState.entered += onRaceGameStateEntered;
            _raceGameState.entered += SetUpSoundSettings;
            _raceGameState.exited += onRaceGameStateExited;

            foreach (TimerGate gate in _gates)
            {
                gate.passed += OnGatePassed;
            }
            foreach (Garage garage in _garages)
            {
                garage.passed += OnGaregePassed;
            }
        }

        public void Dispose()
        {
            _raceGameState.entered -= onRaceGameStateEntered;
            _raceGameState.entered -= SetUpSoundSettings;
            _raceGameState.exited -= onRaceGameStateExited;

            foreach (TimerGate gate in _gates)
            {
                gate.passed -= OnGatePassed;
            }

            foreach (Garage garage in _garages)
            {
                garage.passed -= OnGaregePassed;
            }
        }

        private void SetUpSoundSettings()
        {
            if (_gameSettings.IsSoundOn)
            {
                _soundController.SetValue(AudioMixerExposedParameters.VolumeMaster, _gameSettings.MasterVolume);
                _soundController.SetValue(AudioMixerExposedParameters.VolumeBackgroundMusic, _gameSettings.BackgroundMusicVolume);
                _soundController.SetValue(AudioMixerExposedParameters.VolumeSFX, _gameSettings.SFXSoundVolume);
            }
            else
            {
                _soundController.SoundOff();
            }
        }

        private void OnGatePassed(float time)
        {
            _soundController.Play(SoundID.SFXGate);
        }

        private void OnGaregePassed(bool passed)
        {
            _soundController.Play(SoundID.SFXGarage);
        }

        private void onRaceGameStateEntered()
        {
            _soundController.Play(SoundID.BacgrondMusic);
        }

        private void onRaceGameStateExited()
        {
            _soundController.Stop(SoundID.BacgrondMusic);
        }
    }
}
