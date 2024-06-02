using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Common.Sound
{
    [CreateAssetMenu(menuName = "Sounds")]
    public class Sounds : ScriptableObject
    {
        [SerializeField] private SoundData[] AudioClips;

        public AudioClip GetAudio(SoundID id)
        {
            foreach (SoundData sound in AudioClips)
            {
                if (sound.SoundID == id)
                    return sound.AudioClip;
            }
            throw new ArgumentException($"Can't find sound: {id}");
        }

        public AudioMixerGroup GetAudioMixerGroup(SoundID id)
        {
            foreach (SoundData sound in AudioClips)
            {
                if(sound.SoundID == id)
                    return sound.AudioMixerGroup;
            }
            throw new ArgumentException($"Can't find AudioMixerGroup: {id}");
        }

        [Serializable]
        private class SoundData
        {
            [field: SerializeField] public SoundID SoundID { get; private set; }
            [field: SerializeField] public AudioClip AudioClip { get; private set; }
            [field: SerializeField] public AudioMixerGroup AudioMixerGroup { get; private set; }
            
        }
    }
}


