using UnityEngine;

namespace GamePlay.CarFallingHangling
{
    public interface ICarFallingHandler
    {
        public void HandleFalling(Vector3 lastCarPosition, Quaternion lastCarRotation);
    }
}
