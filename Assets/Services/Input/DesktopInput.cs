using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class DesktopInput: IPlayerInput
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;
        #if DEBUG
        public event Action debugConsoleToggled;
        #endif

        private bool _enabled;
        private bool _paused;

        public void Disable()
        {
            horizontalInput?.Invoke(0);
            brakeInput?.Invoke(false);
            _enabled = false;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Update() 
        {
            if (!_enabled || _paused)
                return;

            horizontalInput?.Invoke(Input.GetAxisRaw("Horizontal"));
            brakeInput?.Invoke(Input.GetKey(KeyCode.Space));

            if (Input.GetMouseButton(0))
                screenInput?.Invoke(Input.mousePosition);

            #if DEBUG
            if (Input.GetKeyDown(KeyCode.Q))
                debugConsoleToggled?.Invoke();
            #endif
        }

        public void Pause() => _paused = true;

        public void Resume() => _paused = false;
    }
}
