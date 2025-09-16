using System;
using Common.DeviceTypeHandling;
using Services.InputService.Desktop;
using Services.InputService.Mobile;
using UnityEngine;

namespace Services.InputService
{
    public class InputSourceFactory : IInputSourceFactory
    {
        private readonly IDeviceTypeHandler _deviceTypeHandler;
        private readonly IBrakeButtonFactory _brakeButtonFactory;

        public InputSourceFactory(IDeviceTypeHandler deviceTypeHandler)
        {
            _deviceTypeHandler = deviceTypeHandler;
        }

        public IInputSource Get()
        {
            DeviceType deviceType = _deviceTypeHandler.GetDeviceType();
            
            switch (deviceType)
            {
                case DeviceType.Desktop:
                    return new DesktopInputSource();
                
                case DeviceType.Handheld:
                    IBrakeButton brakeButton = _brakeButtonFactory.Get();
                    return new MobileInputSource(brakeButton);
                
                default:
                    throw new ArgumentException($"The game doesn't support device type: {deviceType}");
            }
        }
    }
}