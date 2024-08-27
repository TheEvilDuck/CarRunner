using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class MobileInput: IPlayerInput
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;
        #if DEBUG
        public event Action debugConsoleToggled;
        #endif

        private IBrakeButton _brake;
        private readonly Func<IBrakeButton> _brakeButtonFactory;
        private bool _enabled;

        public MobileInput(Func<IBrakeButton> brakeButtonFactory) 
        {
            _brakeButtonFactory = brakeButtonFactory;
        }

        public void Update()
        {
            if (!_enabled)
                return;

            bool isBrake = _brake.IsBraking;
            float horizontalDirection = 0f;

            if (Input.touchCount>0)
            {
                foreach(Touch touch in Input.touches)
                {
                    screenInput?.Invoke(touch.position);

                    if (_brake.IsScreenPositionInside(touch.position))
                    {
                        continue;
                    }

                    if (touch.position.x <= Screen.width / 2f)
                        horizontalDirection = -1;
                    else
                        horizontalDirection = 1;
                }

                #if DEBUG
                if (Input.touchCount == 4)
                    debugConsoleToggled?.Invoke();
                #endif
            }

            horizontalInput?.Invoke(horizontalDirection);
            brakeInput?.Invoke(isBrake);
        }

        public void Enable()
        {
            if (_brake == null || _brake is UnityEngine.Object value && value == null)
                _brake = _brakeButtonFactory.Invoke();

            _enabled = true;
            _brake.Enable();
        }

        public void Disable()
        {
            _enabled = false;
            _brake.Disable();
        }
    }
}
