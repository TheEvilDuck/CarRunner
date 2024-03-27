using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cars
{
    public interface IReadOnlyWheel
    {
        public Vector3 WorldPosition {get;}
        public Quaternion WorldRotation {get;}
    }
}
