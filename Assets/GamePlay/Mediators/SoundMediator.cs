using Common;
using Common.Sound;
using Common.States;
using DI;
using Gameplay.Garages;
using Gameplay.TimerGates;
using Levels;
using MainMenu;
using System;
using System.Collections.Generic;

namespace Gameplay
{
    public class SoundMediator : IDisposable
    {
        private SoundController _soundController;
        private IEnumerable<TimerGate> _gates;
        private IEnumerable<Garage> _garages;
        
        public SoundMediator(DIContainer sceneContext)
        {
            
            _soundController = sceneContext.Get<SoundController>();
            var level = sceneContext.Get<Level>();
            _gates = level.TimerGates;
            _garages = level.Garages;

            foreach (TimerGate gate in _gates)
                gate.passed += OnGatePassed;

            foreach (Garage garage in _garages)
                garage.passed += OnGaregePassed;
        }

        public void Dispose()
        {
            foreach (TimerGate gate in _gates)
                gate.passed -= OnGatePassed;

            foreach (Garage garage in _garages)
                garage.passed -= OnGaregePassed;
        }

        private void OnGatePassed(float time) => _soundController.Play(SoundID.SFXGate);

        private void OnGaregePassed() => _soundController.Play(SoundID.SFXGarage);
    }
}
