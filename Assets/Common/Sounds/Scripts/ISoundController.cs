namespace Common.Sounds.Scripts
{
    public interface ISoundController: IPausable
    {
        public void SoundOff();
        public void Play(SoundID soundID);
        public bool IsPlaying(SoundID soundID);
        public bool IsSoundExisting(SoundID soundID);
        public void Stop(SoundID soundID);
        public void StopAll();
        public void SetValue(AudioMixerExposedParameters param, float normalizedValue);
        public void Init();
    }
}