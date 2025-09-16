using UnityEngine;

namespace Services.InputService.Mobile
{
    public class BrakeButtonFactory : IBrakeButtonFactory
    {
        private const string BRAKE_BUTTON_RESOURCES_PATH = "Prefabs/BrakeButton";

        private readonly Transform _brakeButtonParent;

        public BrakeButtonFactory(Transform brakeButtonParent)
        {
            _brakeButtonParent = brakeButtonParent;
        }

        public IBrakeButton Get()
        {
            return Object.Instantiate(Resources.Load<BrakeButton>(BRAKE_BUTTON_RESOURCES_PATH), _brakeButtonParent);
        }
    }
}