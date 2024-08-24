using Common;
using UnityEngine;

namespace Services.PlayerInput
{
    public interface IBrakeButton: IIsInputInsideChecker
    {
        public bool IsBraking {get;}
        public void Enable();
        public void Disable();
        public void SetParent(Transform parent);
        
    }
}
