using UnityEngine;

namespace Common.Sound
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _backgroundAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private Sounds _sound;


        public void PlayBackgroundMusic()
        {
            _backgroundAudioSource.clip = _sound.GetAudio(SoundID.BacgrondMusic);
            _backgroundAudioSource.Play();
        }

        public void PlaySFXGarage()
        {
            _sfxAudioSource.clip = _sound.GetAudio(SoundID.SFXGarage);
            _sfxAudioSource.Play();
        }

        public void PlaySFXGate()
        {
            _sfxAudioSource.clip = _sound.GetAudio(SoundID.SFXGate);
            _sfxAudioSource.Play();
        }
    }
}

