using System;

namespace Common
{
    public interface ICameraSettings : ISettings
    {
        public float AngleOfView {get;}
        public float ZOffset {get;}
        public event Action CameraSettingsChanged;

        public void SetAngleOfView(float value);
        public void SetZOffset(float value);
    }
}