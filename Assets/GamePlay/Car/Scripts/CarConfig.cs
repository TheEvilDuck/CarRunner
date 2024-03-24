using UnityEngine;

[CreateAssetMenu(fileName = "CarConfig", menuName = "Car/Config")]
public class CarConfig : ScriptableObject
{
    [field:SerializeField]public float Acceleration { get; private set; }
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public GameObject ModelOfCar { get; private set; }
}
