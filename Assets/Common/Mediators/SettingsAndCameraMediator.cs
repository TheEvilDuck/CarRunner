using System;
using Common.Components;
using DI;
using UnityEngine;

namespace Common.Mediators
{
    public class SettingsAndCameraMediator : IDisposable
    {
        private Camera _camera;
        private GameSettings _gameSettings;
        private CameraFollow _cameraFollow;
        private RangeOfCameraSettings _rangeOfCameraSettings;

        public SettingsAndCameraMediator(DIContainer sceneContext)
        {
            _camera = sceneContext.Get<Camera>();
            _gameSettings = sceneContext.Get<GameSettings>();
            _cameraFollow = sceneContext.Get<CameraFollow>();
            _rangeOfCameraSettings = sceneContext.Get<RangeOfCameraSettings>();

            UpdateCameraSettings();
            _gameSettings.CameraSettingsChanged += UpdateCameraSettings;
        }

        public void Dispose()
        {
            _gameSettings.CameraSettingsChanged -= UpdateCameraSettings;
        }

        private void UpdateCameraSettings()
        {
            int angleOfView = (int)_rangeOfCameraSettings.AngleOfView.GetConvertedValue(_gameSettings.AngleOfView);
            float zOffset = _rangeOfCameraSettings.ZOffset.GetConvertedValue(_gameSettings.ZOffset);

            _camera.fieldOfView = angleOfView;
            _cameraFollow.SetZOffset(zOffset);
        }
    }
}

