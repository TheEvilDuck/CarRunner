using System;
using UnityEngine;

namespace Services.InputService.Desktop
{
    public class DesktopInputSource: IInputSource
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;

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
        }
    }
}
