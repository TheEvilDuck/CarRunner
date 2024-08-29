using System;
using Common;
using UnityEngine;

namespace Services.PlayerInput
{
    public interface IPlayerInput: IPausable
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;
        #if DEBUG
        public event Action debugConsoleToggled;
        #endif
        public void Update();
        public void Enable();
        public void Disable();
    }
}
