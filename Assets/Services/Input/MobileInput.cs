using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class MobileInput: IPlayerInput
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;

        private RectTransform _brake;

        public MobileInput(RectTransform brake) 
        {
            _brake = brake;
        }

        public void Update()
        {
            bool isBrake = false;
            float horizontalDirection = 0f;

            if (Input.touchCount>0)
            {
                foreach(Touch touch in Input.touches)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(_brake, touch.position))
                    {
                        isBrake = true;
                    }
                    else
                    {
                        screenInput?.Invoke(touch.position);

                        if (touch.position.x <= Screen.width / 2f)
                            horizontalDirection = -1;
                        else
                            horizontalDirection = 1;
                    }
                }
            }

            horizontalInput?.Invoke(horizontalDirection);
            brakeInput?.Invoke(isBrake);
        }

        public void Enable()
        {
            _brake.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _brake.gameObject.SetActive(false);
        }
    }
}
