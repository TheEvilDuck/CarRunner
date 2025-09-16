namespace Services.TimerService
{
    public interface ITimerService
    {
        public float DeltaTime { get; }
        public float CurrentTime { get; }
    }
}