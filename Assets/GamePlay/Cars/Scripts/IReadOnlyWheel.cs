using UnityEngine;

namespace Gameplay.Cars
{
    public interface IReadOnlyWheel
    {
        public Vector3 WorldPosition {get;}
        public Quaternion WorldRotation {get;}
        public float Radius {get;}
    }
}
