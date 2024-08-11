using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public struct Range
    {
        [field: SerializeField] public float MinValue { get; private set; }
        [field: SerializeField] public float MaxValue { get; private set; }

        public float GetConvertedValue(float normalizedValue)
        {
            normalizedValue = Mathf.Clamp(normalizedValue, 0, 1);
            return MinValue + normalizedValue * Mathf.Abs(MaxValue - MinValue);
        }

        public void Validate()
        {
            if(MinValue > MaxValue)
            {
                Debug.LogWarning("MinValue must not be greater than MaxValue");
                MinValue = Mathf.Min(MinValue, MaxValue);
            }
        }
    }
}
