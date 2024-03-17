using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public interface IPlayerInput
    {
        public event Action<float> horizontalInput;
        public void Update();
    }
}
