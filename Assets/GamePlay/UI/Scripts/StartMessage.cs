using Common.UI.UIAnimations;
using UnityEngine;

namespace Gameplay.UI
{
    public class StartMessage : MonoBehaviour
    {
        [SerializeField] private UIAnimatorSequence _animation;

        public void Show()
        {
            gameObject.SetActive(true);
            _animation.StartSequence();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _animation.StopSequence();
        }
    }
}
