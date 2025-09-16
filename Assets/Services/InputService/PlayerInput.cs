using System;
using Common;
using Common.Tickables;
using UnityEngine;

namespace Services.InputService
{
    public class PlayerInput: IPausable, IDisposable, ITickable
    {
        public event Action<float> horizontalInput;
        public event Action<bool> brakeInput;
        public event Action<Vector2> screenInput;

        private bool _paused = false;
        private bool _enabled;
        
        private IInputSource _inputSource;

        public void SwitchInputSource(IInputSource inputSource)
        {
            CleanUp();
            
            _inputSource = inputSource;
            
            _inputSource.horizontalInput += OnHorizontalInput;
            _inputSource.brakeInput += OnBrakeInput;
            _inputSource.screenInput += OnScreenInput;
            
            if (_enabled)
                _inputSource.Enable();
        }

        public void Dispose() => CleanUp();

        public void Tick(float deltaTime)
        {
            if (_paused || _enabled == false)
                return;
            
            _inputSource?.Update();
        }
        public void Enable()
        {
            _enabled = true;
            _inputSource?.Enable();
        }

        public void Disable()
        {
            _enabled = false;
            _inputSource?.Disable();
        }

        public void Pause() => _paused = true;
        public void Resume() => _paused = false;
        
        private void OnScreenInput(Vector2 input) => screenInput?.Invoke(input);
        private void OnBrakeInput(bool isBraking) => brakeInput?.Invoke(isBraking);
        private void OnHorizontalInput(float horizontal) => horizontalInput?.Invoke(horizontal);

        private void CleanUp()
        {
            if (_inputSource != null)
            {
                _inputSource.Disable();

                _inputSource.horizontalInput -= OnHorizontalInput;
                _inputSource.brakeInput -= OnBrakeInput;
                _inputSource.screenInput -= OnScreenInput;
            }
        }
    }
}
