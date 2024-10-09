using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Common.Sound
{
    public class SoundController : MonoBehaviour, IPausable, IDisposable
    {
        private const float MUTE_VOLUME = -80f;
        [SerializeField] private Sounds _sound;
        [SerializeField] private GameObject _objectToPool;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private RangeOfExposedParameters _rangeOfExposedParameters;
        private Dictionary<AudioMixerExposedParameters, string> _audioMixerExposedParameters = new Dictionary<AudioMixerExposedParameters, string>()
        {
            {AudioMixerExposedParameters.PitchBackgroundMusic, "PitchBackgroundMusic" },
            {AudioMixerExposedParameters.VolumeBackgroundMusic, "VolumeBackgroundMusic" },
            {AudioMixerExposedParameters.VolumeMaster, "VolumeMaster" },
            {AudioMixerExposedParameters.PitchMaster, "PitchMaster" },
            {AudioMixerExposedParameters.PitchSFX, "PitchSFX" },
            {AudioMixerExposedParameters.VolumeSFX, "VolumeSFX" }
        };
        private ObjectPool<AudioSource> _audioSourcePool;
        private List<AudioSource> _usedObjects = new List<AudioSource>();
        private bool _collectionCheck = false;
        private int _poolDefaultCapacity = 5;
        private int _poolMaxSize = 5;
        private bool _paused;

        private void LateUpdate()
        {
            if (_usedObjects.Count > 0)
            {
                for (int i = _usedObjects.Count - 1; i > -1; i--)
                {
                    if (_usedObjects[i] == null || !_usedObjects[i].isPlaying && !_paused)
                    {
                        _audioSourcePool.Release(_usedObjects[i]);
                    }
                }
            }
        }

        public void Init()
        {
            _audioSourcePool = new ObjectPool<AudioSource>(CreatePooledObject, TakeFromPool, ReturnToPool, DestroyObject, _collectionCheck, _poolDefaultCapacity, _poolMaxSize);
        }

        public void SoundOff()
        {
            _audioMixer.SetFloat(_audioMixerExposedParameters[AudioMixerExposedParameters.VolumeMaster], MUTE_VOLUME);
        }

        public void Play(SoundID soundID)
        {
            AudioSource audioSource = _audioSourcePool.Get();
            audioSource.outputAudioMixerGroup = _sound.GetAudioMixerGroup(soundID);
            audioSource.clip = _sound.GetAudio(soundID);
            audioSource.loop = _sound.IsLooped(soundID);
            audioSource.Play();
            _usedObjects.Add(audioSource);

            if (_paused)
                audioSource.Pause();
        }

        public bool IsPlaying(SoundID soundID)
        {
            if (_usedObjects == null)
                return false;

            if (_usedObjects.Count == 0)
                return false;

            if (_paused)
                return false;

            for (int i = _usedObjects.Count - 1; i > -1; i--)
            {
                if (_usedObjects[i] != null && _usedObjects[i].clip == _sound.GetAudio(soundID))
                {
                    return _usedObjects[i].isPlaying;
                }
            }

            return false;
        }

        public bool IsSoundExisting(SoundID soundID)
        {
            if (_usedObjects == null)
                return false;

            if (_usedObjects.Count == 0)
                return false;

            if (_paused)
                return false;

            for (int i = _usedObjects.Count - 1; i > -1; i--)
            {
                if (_usedObjects[i] != null && _usedObjects[i].clip == _sound.GetAudio(soundID))
                {
                    return true;
                }
            }

            return false;
        }

        public void Stop(SoundID soundID)
        {
            if (_usedObjects.Count > 0)
            {
                for (int i = _usedObjects.Count - 1; i > -1; i--)
                {
                    if (_usedObjects[i] != null && _usedObjects[i].clip == _sound.GetAudio(soundID))
                    {
                        _audioSourcePool.Release(_usedObjects[i]);
                    }
                }
            }
        }

        public void StopAll()
        {
            for (int i = _usedObjects.Count - 1; i > -1; i--)
            {
                if (_usedObjects[i] != null)
                {
                    _audioSourcePool.Release(_usedObjects[i]);
                }
            }
        }

        public void SetValue(AudioMixerExposedParameters param, float normalizedValue)
        {
            var convertedValue = _rangeOfExposedParameters.GetRange(param, normalizedValue);

            if (normalizedValue == 0)
            {
                convertedValue = MUTE_VOLUME;
            }

            _audioMixer.SetFloat(_audioMixerExposedParameters[param], convertedValue);
        }

        public void Pause()
        {
            _paused = true;

            foreach (var sound in _usedObjects)
            {
                sound.Pause();
            }
        }

        public void Resume()
        {
            _paused = false;

            foreach (var sound in _usedObjects)
            {
                if (sound == null)
                    continue;

                sound.UnPause();
            }
        }

        public void Dispose()
        {
            StopAll();
            _audioSourcePool.Dispose();
        }

        private AudioSource CreatePooledObject()
        {
            GameObject soundObject = Instantiate(_objectToPool);
            soundObject.transform.SetParent(this.transform);
            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.gameObject.SetActive(false);
            return audioSource;
        }

        private void TakeFromPool(AudioSource audioSource) => audioSource.gameObject.SetActive(true);

        private void ReturnToPool(AudioSource audioSource)
        {
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.gameObject.SetActive(false);
            _usedObjects.Remove(audioSource);
        }

        private void DestroyObject(AudioSource audioSource) => Destroy(audioSource.gameObject);
    }
}

