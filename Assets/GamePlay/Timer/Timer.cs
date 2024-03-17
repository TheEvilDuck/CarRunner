using System;
using Common;
using UnityEngine;

namespace Gameplay
{
    public class Timer: IPausable
    {
        public event Action end;
        public event Action started;
        public event Action<float> changed;

        private bool _paused;
        private bool _end;
        private readonly float _startTime;

        public float CurrentTime {get; private set;}

        public Timer(float startTime)
        {
            CurrentTime = startTime;
            _startTime = startTime;

            started?.Invoke();
        }

        public void Update()
        {
            if (_paused||_end)
                return;

            CurrentTime -= Time.deltaTime;

            if (CurrentTime<=0)
            {
                CurrentTime = 0;
                end?.Invoke();
                _end = true;
            }

            changed?.Invoke(CurrentTime);
        }

        public void Restart()
        {
            _end = false;
            _paused = false;
            CurrentTime = _startTime;

            started?.Invoke();
        }

        public void OffsetTime(float amount)
        {
            if (_end)
                return;

            CurrentTime+=amount;
            changed?.Invoke(CurrentTime);
        }

        public void Pause() => _paused = true;

        public void Resume() => _paused = false;
    }
}
