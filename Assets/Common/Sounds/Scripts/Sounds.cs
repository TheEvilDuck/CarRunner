using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Common.Sound
{
    [CreateAssetMenu(menuName = "Sounds")]
    public class Sounds : ScriptableObject
    {
        [SerializeField] private SoundData[] AudioClips;

        public AudioClip GetAudio(SoundID id) => Find(id).AudioClip;

        public AudioMixerGroup GetAudioMixerGroup(SoundID id) => Find(id).AudioMixerGroup;

        public bool IsLooped(SoundID id) => Find(id).Loop;

        private SoundData Find(SoundID soundID)
        {
            foreach (SoundData sound in AudioClips)
            {
                if(sound.SoundID == soundID)
                    return sound;
            }
            throw new ArgumentException($"Can't find AudioMixerGroup: {soundID}");
        }

        [Serializable]
        private class SoundData
        {
            [field: SerializeField] public SoundID SoundID { get; private set; }
            [field: SerializeField] public AudioClip AudioClip { get; private set; }
            [field: SerializeField] public AudioMixerGroup AudioMixerGroup { get; private set; }
            [field: SerializeField] public bool Loop {get; private set;}
        }
    }
}


