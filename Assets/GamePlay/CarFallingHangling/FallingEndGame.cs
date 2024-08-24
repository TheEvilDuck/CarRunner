using System;
using UnityEngine;

namespace Gameplay.CarFallingHandling
{
    public class FallingEndGame : ICarFallingHandler
    {
        public event Action falled;
        public void HandleFalling(Vector3 lastCarPosition, Quaternion lastCarRotation) => falled?.Invoke();
    }
}
