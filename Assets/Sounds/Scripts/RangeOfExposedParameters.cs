using System;
using UnityEngine;

namespace Common.Sound
{
    [CreateAssetMenu(menuName = "RangeOfExposedParameters")]
    public class RangeOfExposedParameters : ScriptableObject
    {
        [SerializeField] private Range[] _range;

        public Range GetRange(AudioMixerExposedParameters parameter)
        {
            foreach(Range range in _range)
            {
                if(range.Parameter == parameter)
                    return range;
            }
            throw new ArgumentException($"Can't find Audio Mixer Exposed Parameter: {parameter}");
        }

        private void OnValidate()
        {
            foreach(Range range in _range)
                if(range.MinValue > range.MaxValue)
                    throw new ArgumentException($"MinValue > MaxValue: {range.Parameter}");
        }

        [Serializable]
        public struct Range
        {
            [field: SerializeField] public AudioMixerExposedParameters Parameter { get; private set; }
            [field: SerializeField] public float MinValue { get; private set; }
            [field: SerializeField] public float MaxValue { get; private set; }
        }
    }
}
