using UnityEngine;

namespace GamePlay.Cars.Scripts
{
    public interface IReadOnlyWheel
    {
        public Vector3 WorldPosition {get;}
        public Quaternion WorldRotation {get;}
        public float Radius {get;}
        public bool IsLeft {get;}
    }
}
