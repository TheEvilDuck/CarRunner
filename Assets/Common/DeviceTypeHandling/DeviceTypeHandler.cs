using UnityEngine;

namespace Common.DeviceTypeHandling
{
    public class DeviceTypeHandler : IDeviceTypeHandler
    {
        public DeviceType GetDeviceType()
        {
            DeviceType deviceType = DeviceType.Desktop;

#if UNITY_WEBGL
            if (YandexGame.EnvironmentData.isDesktop)
                deviceType = DeviceType.Desktop;
            else if (YandexGame.EnvironmentData.isMobile)
                deviceType = DeviceType.Handheld;
#endif

#if UNITY_STANDALONE_WIN
            deviceType = DeviceType.Desktop;
#endif

#if UNITY_ANDROID
            deviceType = DeviceType.Handheld;
#endif

#if UNITY_EDITOR
            deviceType = DeviceType.Desktop;
            //deviceType = DeviceType.Handheld;
#endif

            return deviceType;
        }
    }
}