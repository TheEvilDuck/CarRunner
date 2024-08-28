using Common;
using Common.UI.UIAnimations;
using UnityEngine;

namespace Gameplay.UI
{
    public class StartMessage : MonoBehaviour, IPausable
    {
        [SerializeField] private UIAnimatorSequence _animation;

        private bool _showed;

        public void Show()
        {
            gameObject.SetActive(true);
            _animation.StartSequence();
            _showed = true;
        }

        public void Hide()
        {
            _animation.StopSequence();
            _showed = false;
            gameObject.SetActive(false);
        }

        public void Pause()
        {
            _animation.StopSequence();
            gameObject.SetActive(false);  
        }

        public void Resume()
        {
            if (_showed)
            {
                gameObject.SetActive(true);
                _animation.StartSequence();
            }
        }
    }
}
