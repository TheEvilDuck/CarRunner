using System;
using System.Collections.Generic;
using DI;
using Levels;

namespace Gameplay.TimerGates
{
    public class TimerAndGatesMediator : IDisposable
    {
        private readonly IEnumerable<TimerGate> _timerGates;
        private readonly Timer _timer;

        public TimerAndGatesMediator(DIContainer sceneContext)
        {
            _timerGates = sceneContext.Get<Level>().TimerGates;
            _timer = sceneContext.Get<Timer>();

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
