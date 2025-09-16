using System;
using UnityEngine;

namespace Services.InputService.Mobile
{
    public class MobileInputSource: IInputSource
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;

        private IBrakeButton _brakeButton;
        
        private bool _enabled;
        private bool _paused;

        public MobileInputSource(IBrakeButton brakeButton)
        {
            _brakeButton = brakeButton;
        }

        public void Update()
        {
            if (!_enabled || _paused)
                return;

            bool isBrake = _brakeButton.IsBraking;
            float horizontalDirection = 0f;

            if (Input.touchCount > 0)
            {
                foreach(Touch touch in Input.touches)
                {
                    screenInput?.Invoke(touch.position);

                    if (_brakeButton.IsScreenPositionInside(touch.position))
                    {
                        continue;
                    }

                    if (touch.position.x <= Screen.width / 2f)
                        horizontalDirection = -1;
                    else
                        horizontalDirection = 1;
                }
            }

            horizontalInput?.Invoke(horizontalDirection);
            brakeInput?.Invoke(isBrake);
        }

        public void Enable()
        {
            _enabled = true;
            _brakeButton.Enable();
        }

        public void Disable()
        {
            _enabled = false;
            _brakeButton.Disable();
        }

        public void Pause() => _paused = true;

        public void Resume() => _paused = false;
    }
}
