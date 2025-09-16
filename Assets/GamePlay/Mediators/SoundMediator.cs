using System;
using System.Collections.Generic;
using Common.Sounds.Scripts;
using GamePlay.TimerGates.Scripts;
using Infrastructure.DI;
using Levels.Scripts;

namespace GamePlay.Mediators
{
    public class SoundMediator : IDisposable
    {
        private ISoundController _soundController;
        private IEnumerable<TimerGate> _gates;
        private IEnumerable<Garage.Scripts.Garage> _garages;
        
        public SoundMediator(IDIContainer sceneContext)
        {
            
            _soundController = sceneContext.Get<ISoundController>();
            var level = sceneContext.Get<Level>();
            _gates = level.TimerGates;
            _garages = level.Garages;

            foreach (TimerGate gate in _gates)
                gate.passed += OnGatePassed;

            foreach (Garage.Scripts.Garage garage in _garages)
                garage.passed += OnGaregePassed;
        }

        public void Dispose()
        {
            foreach (TimerGate gate in _gates)
                gate.passed -= OnGatePassed;

            foreach (Garage.Scripts.Garage garage in _garages)
                garage.passed -= OnGaregePassed;
        }

        private void OnGatePassed(float time) => _soundController.Play(SoundID.SFXGate);

        private void OnGaregePassed() => _soundController.Play(SoundID.SFXGarage);
    }
}
