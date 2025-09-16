using System;
using Infrastructure.DI;

namespace GamePlay.Timer
{
    public class TimerMediator : IDisposable
    {
        private Timer _timer;
        private TimerView _timerView;

        public TimerMediator(IDIContainer sceneContext)
        {
            _timer = sceneContext.Get<Timer>();
            _timerView = sceneContext.Get<TimerView>();

            _timer.changed+=OnTimerChanged;
            _timer.started+=OnTimerStarted;
            _timer.end+=OnTimerEnd;

            _timerView.Hide();
        }
        public void Dispose()
        {
            _timer.changed-=OnTimerChanged;
            _timer.started-=OnTimerStarted;
            _timer.end-=OnTimerEnd;
        }

        private void OnTimerChanged(float time) => _timerView.ChangeValue(time);
        private void OnTimerStarted() => _timerView.Show();
        private void OnTimerEnd() => _timerView.Hide();
    }
}
