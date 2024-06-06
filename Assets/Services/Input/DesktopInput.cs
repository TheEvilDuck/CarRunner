using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class DesktopInput: IPlayerInput
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;

        public void Update() 
        {
            horizontalInput?.Invoke(Input.GetAxisRaw("Horizontal"));
            brakeInput?.Invoke(Input.GetKey(KeyCode.Space));
        }
    }
}
