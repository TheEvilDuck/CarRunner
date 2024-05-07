using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.Sound
{
    //++++++++++++++++++++++++++
    //где удалять пул? : посмотреть в документации удаление ObjectPool
    //++++++++++++++++++++++++++
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _backgroundAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private Sounds _sound;
        [SerializeField] private GameObject _objectToPool;
        private ObjectPool<GameObject> _audioSourcePool;
        private List<GameObject> _usedObjects = new List<GameObject>();
        private bool _collectionCheck = false;
        private int _poolDefaultCapacity = 5;
        private int _poolMaxSize = 5;

        private void LateUpdate()
        {
            if (_usedObjects.Count > 0)
            {
                for (int i = _usedObjects.Count - 1; i > -1; i--)
                {
                    AudioSource audioSource = _usedObjects[i].GetComponent<AudioSource>();
                    if (!audioSource.isPlaying)
                    {
                        _audioSourcePool.Release(_usedObjects[i]);
                        _usedObjects.RemoveAt(i);
                    }
                }
            }
        }

        public void Init()
        {
            //_audioSourcePool = new ObjectPool<GameObject>(createFunc: CreatePooledObject, actionOnGet: TakeFromPool, actionOnRelease: ReturnToPool, actionOnDestroy: DestroyObject, collectionCheck: false, defaultCapacity: 5, maxSize: 5);
            _audioSourcePool = new ObjectPool<GameObject>(CreatePooledObject, TakeFromPool, ReturnToPool, DestroyObject, _collectionCheck, _poolDefaultCapacity, _poolMaxSize);
        }

        public void PlayBackgroundMusic()
        {
            _backgroundAudioSource.clip = _sound.GetAudio(SoundID.BacgrondMusic);
            _backgroundAudioSource.Play();
        }

        public void StopBackgroundMusic()
        {
            _backgroundAudioSource.Stop();
        }

        public void PlaySFXGarage()
        {
            GameObject soundObject = _audioSourcePool.Get();
            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.clip = _sound.GetAudio(SoundID.SFXGarage);
            audioSource.Play();
            _usedObjects.Add(soundObject);
        }

        public void PlaySFXGate()
        {
            GameObject soundObject = _audioSourcePool.Get();
            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.clip = _sound.GetAudio(SoundID.SFXGate);
            audioSource.Play();
            _usedObjects.Add(soundObject);
        }

        private GameObject CreatePooledObject()
        {
            GameObject soundObject = Instantiate(_objectToPool);
            soundObject.SetActive(false);
            soundObject.transform.SetParent(this.transform);
            return soundObject;
        }

        private void TakeFromPool(GameObject soundObject)
        {
            soundObject.SetActive(true);
        }

        private void ReturnToPool(GameObject soundObject)
        {
            soundObject.SetActive(false);
        }

        private void DestroyObject(GameObject soundObject)
        {
            Destroy(soundObject);
        }

    }
}

