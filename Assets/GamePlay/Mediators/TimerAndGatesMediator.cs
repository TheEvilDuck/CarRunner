using System;
using System.Collections.Generic;
using GamePlay.TimerGates.Scripts;
using Infrastructure.DI;
using Levels.Scripts;

namespace GamePlay.Mediators
{
    public class TimerAndGatesMediator : IDisposable
    {
        private readonly IEnumerable<TimerGate> _timerGates;
        private readonly Timer.Timer _timer;

        public TimerAndGatesMediator(IDIContainer sceneContext)
        {
            _timerGates = sceneContext.Get<Level>().TimerGates;
            _timer = sceneContext.Get<Timer.Timer>();

            foreach (TimerGate timerGate in _timerGates)
            {
                timerGate.passed+=OnGatePass;
            }
        }

        public void Dispose()
        {
            foreach (TimerGate timerGate in _timerGates)
            {
                timerGate.passed-=OnGatePass;
            }
        }

        private void OnGatePass(float time)
        {
            _timer.OffsetTime(time);
        }
    }
}
