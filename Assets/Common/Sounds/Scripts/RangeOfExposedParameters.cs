using System;
using UnityEngine;

namespace Common.Sound
{
    [CreateAssetMenu(menuName = "RangeOfExposedParameters")]
    public class RangeOfExposedParameters : ScriptableObject
    {
        [SerializeField] private RangeData[] _range;

        public float GetRange(AudioMixerExposedParameters parameter, float normalizedValue)
        {
            foreach(RangeData rangeData in _range)
            {
                if(rangeData.Parameter == parameter)
                    return rangeData.Range.GetConvertedValue(normalizedValue);
            }
            throw new ArgumentException($"Can't find Audio Mixer Exposed Parameter: {parameter}");
        }

        private void OnValidate()
        {
            foreach(RangeData rangeData in _range)
                rangeData.Range.Validate();      
        }

        [Serializable]
        private struct RangeData
        {
            [field: SerializeField] public AudioMixerExposedParameters Parameter { get; private set; }
            [field: SerializeField] public Range Range;
        }
    }
}
