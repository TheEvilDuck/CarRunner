using System;
using Common.Components;
using DI;
using UnityEngine;

namespace Common.Mediators
{
    public class SettingsAndCameraMediator : IDisposable
    {
        private Camera _camera;
        private ICameraSettings _cameraSettings;
        private CameraFollow _cameraFollow;
        private RangeOfCameraSettings _rangeOfCameraSettings;

        public SettingsAndCameraMediator(DIContainer sceneContext)
        {
            _cameraSettings = sceneContext.Get<ICameraSettings>();
            _camera = sceneContext.Get<Camera>();
            _cameraFollow = sceneContext.Get<CameraFollow>();
            _rangeOfCameraSettings = sceneContext.Get<RangeOfCameraSettings>();

            UpdateCameraSettings();
            _cameraSettings.CameraSettingsChanged += UpdateCameraSettings;
        }

        public void Dispose()
        {
            _cameraSettings.CameraSettingsChanged -= UpdateCameraSettings;
        }

        private void UpdateCameraSettings()
        {
            int angleOfView = (int)_rangeOfCameraSettings.AngleOfView.GetConvertedValue(_cameraSettings.AngleOfView);
            float zOffset = _rangeOfCameraSettings.ZOffset.GetConvertedValue(_cameraSettings.ZOffset);

            _camera.fieldOfView = angleOfView;
            _cameraFollow.SetZOffset(zOffset);
        }
    }
}

