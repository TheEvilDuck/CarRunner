using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

namespace Common.Sound
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private Sounds _sound;
        [SerializeField] private GameObject _objectToPool;
        [SerializeField] private AudioMixer _audioMixer;
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

        private void LateUpdate()
        {
            if (_usedObjects.Count > 0)
            {
                for (int i = _usedObjects.Count - 1; i > -1; i--)
                {
                    if (_usedObjects[i] == null || !_usedObjects[i].isPlaying)
                    {
                        _audioSourcePool.Release(_usedObjects[i]);
                        _usedObjects.RemoveAt(i);
                    }
                }
            }
        }

        public void Init()
        {
            _audioSourcePool = new ObjectPool<AudioSource>(CreatePooledObject, TakeFromPool, ReturnToPool, DestroyObject, _collectionCheck, _poolDefaultCapacity, _poolMaxSize);
        }

        public void Play(SoundID soundID)
        {
            AudioSource audioSource = _audioSourcePool.Get();
            audioSource.outputAudioMixerGroup = _sound.GetAudioMixerGroup(soundID);
            audioSource.clip = _sound.GetAudio(soundID);
            audioSource.Play();
            _usedObjects.Add(audioSource);
        }

        public void Stop(SoundID soundID)
        {
            if (_usedObjects.Count > 0)
            {
                for (int i = _usedObjects.Count - 1; i > -1; i--)
                {
                    if (_usedObjects[i].clip == _sound.GetAudio(soundID))
                        _usedObjects[i].Stop();
                }
            }
        }

        public void SetValue(AudioMixerExposedParameters param, float value)
        {
            _audioMixer.SetFloat(_audioMixerExposedParameters[param], value);
        }

        private AudioSource CreatePooledObject()
        {
            GameObject soundObject = Instantiate(_objectToPool);
            soundObject.transform.SetParent(this.transform);
            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.gameObject.SetActive(false);
            return audioSource;
        }

        private void TakeFromPool(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(true);
        }

        private void ReturnToPool(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(false);
        }

        private void DestroyObject(AudioSource audioSource)
        {
            Destroy(audioSource.gameObject);
        }
    }
}
