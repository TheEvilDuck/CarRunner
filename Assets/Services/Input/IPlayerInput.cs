using System;

namespace Services.PlayerInput
{
    public interface IPlayerInput
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public void Update();
    }
}
