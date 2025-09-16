using System;
using UnityEngine;

namespace GamePlay.CarFallingHangling
{
    public class FallingEndGame : ICarFallingHandler
    {
        public event Action falled;
        public void HandleFalling(Vector3 lastCarPosition, Quaternion lastCarRotation) => falled?.Invoke();
    }
}
