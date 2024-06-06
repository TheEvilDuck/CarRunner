using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class MobileInput: IPlayerInput
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;

        public void Update()
        {
            if (Input.touchCount>0)
            {
                Touch touch = Input.GetTouch(Input.touchCount-1);

                if (touch.position.x<=Screen.width/2f)
                    horizontalInput?.Invoke(-1f);
                else
                    horizontalInput?.Invoke(1f);
            }
            else
            {
                horizontalInput?.Invoke(0);
            }
        }
    }
}
