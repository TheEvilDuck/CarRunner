using System;
using UnityEngine;

namespace Services.InputService
{
    public interface IInputSource
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;

        public void Update();
        public void Enable();
        public void Disable();
    }
}