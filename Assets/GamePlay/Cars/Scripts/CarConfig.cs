using UnityEngine;

namespace Gameplay.Cars
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "Car/Config")]
    public class CarConfig : ScriptableObject
    {
        public const float MAX_SPEED = 200f;
        public const float MAX_ACCELERATION = 10000f;
        [field:SerializeField, Range(0, MAX_ACCELERATION)]public float Acceleration { get; private set; }
        [field: SerializeField, Range(0, MAX_SPEED)] public float MaxSpeed { get; private set; }
        [field: SerializeField] public Material[] Materials {get; private set;}
        [field: SerializeField] public CarView CarViewPrefab {get; private set;}
    }
}
