using UnityEngine;

namespace Gameplay.CarFallingHandling
{
    public interface ICarFallingHandler
    {
        public void HandleFalling(Vector3 lastCarPosition, Quaternion lastCarRotation);
    }
}
