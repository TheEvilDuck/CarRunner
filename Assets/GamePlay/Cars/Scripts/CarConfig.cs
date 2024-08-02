using UnityEngine;

namespace Gameplay.Cars
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "Car/Config")]
    public class CarConfig : ScriptableObject
    {
        [field:SerializeField]public float Acceleration { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
        [field: SerializeField] public Material[] Materials {get; private set;}
        [field: SerializeField] public CarView CarViewPrefab {get; private set;}
    }
}
