using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class AdButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private UIAnimatorSequence _animations;

        public UnityEvent clicked => _button.onClick;
        private void OnEnable() => _animations.StartSequence();
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }

}