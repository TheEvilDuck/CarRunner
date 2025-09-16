using UnityEngine;

namespace Common.DeviceTypeHandling
{
    public interface IDeviceTypeHandler
    {
        public DeviceType GetDeviceType();
    }
}