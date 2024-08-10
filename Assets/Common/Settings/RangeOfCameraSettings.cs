using UnityEngine;

namespace Common
{
    [CreateAssetMenu(menuName = "RangeOfCameraSettings")]
    public class RangeOfCameraSettings : ScriptableObject
    {
        [field: SerializeField] public Range AngleOfView { get; private set; }
        [field: SerializeField] public Range ZOffset { get; private set; }
    }
}