using Common.Sound;
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

        public SoundMediator(SoundController soundController, TimerGate[] gates, Garage[] garages)
        {
            _soundController = soundController;
            _gates = gates;
            _garages = garages;

            foreach (TimerGate gate in gates)
            {
                gate.passed += OnGatePassed;
            }
            foreach (Garage garage in garages)
            {
                garage.passed += OnGaregePassed;
            }
        }

        public void Dispose()
        {
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
    }
}
