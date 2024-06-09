using System;

namespace Gameplay.TimerGates
{
    public class TimerAndGatesMediator : IDisposable
    {
        private TimerGate[] _timerGates;
        private readonly Timer _timer;

        public TimerAndGatesMediator(TimerGate[] timerGates, Timer timer)
        {
            _timerGates = timerGates;
            _timer = timer;

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
