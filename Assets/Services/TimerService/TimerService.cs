using UnityEngine;

namespace Services.TimerService
{
    public class TimerService: ITimerService
    {
        public float DeltaTime => Time.deltaTime;
        public float CurrentTime => Time.time;
    }
}