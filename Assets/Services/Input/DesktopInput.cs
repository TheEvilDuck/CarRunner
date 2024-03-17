using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class DesktopInput: IPlayerInput
    {
        public event Action<float> horizontalInput;

        public void Update() 
        {
            horizontalInput?.Invoke(Input.GetAxisRaw("Horizontal"));
        }
    }
}
