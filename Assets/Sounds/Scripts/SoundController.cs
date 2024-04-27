using System;
using UnityEngine;


public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private Sound[] _sounds;

    public void PlayBackgroundMusic()
    {
        _backgroundAudioSource.clip = _sounds[0]._clip;
        _backgroundAudioSource.Play();
    }

    public void PlaySFXGarage()
    {
        _sfxAudioSource.clip = _sounds[1]._clip;
        _sfxAudioSource.Play();
    }

    public void PlaySFXGate()
    {
        _sfxAudioSource.clip = _sounds[2]._clip;
        _sfxAudioSource.Play();
    }


    [Serializable]
    private class Sound
    {
        [SerializeField]public string _name;
        [SerializeField]public AudioClip _clip;
    }
}
