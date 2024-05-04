using Common.Sound;
using Gameplay.Garages;
using Gameplay.States;
using Gameplay.TimerGates;
using System;

namespace Gameplay
{
    public class SoundMediator : IDisposable
    {
        private SoundController _soundController;
        private TimerGate[] _gates;
        private Garage[] _garages;
        private RaceGameState _raceGameState;

        public SoundMediator(SoundController soundController, TimerGate[] gates, Garage[] garages, RaceGameState raceGameState)
        {
            _soundController = soundController;
            _gates = gates;
            _garages = garages;
            _raceGameState = raceGameState;

            _raceGameState.entered += StartBackgroundMusic;
            _raceGameState.exited += StopBackgroundAudio;

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
            _raceGameState.entered -= StartBackgroundMusic;
            _raceGameState.exited -= StopBackgroundAudio;

            foreach (TimerGate gate in _gates)
            {
                gate.passed -= OnGatePassed;
            }

            foreach (Garage garage in _garages)
            {
                garage.passed -= OnGaregePassed;
            }
        }

        private void OnGatePassed(float time)
        {
            _soundController.PlaySFXGate();
        }

        private void OnGaregePassed(bool passed)
        {
            _soundController.PlaySFXGarage();
        }

        private void StartBackgroundMusic()
        {
            _soundController.PlayBackgroundMusic();
        }

        private void StopBackgroundAudio()
        {
            _soundController.StopBackgroundMusic();
        }
    }
}
