using Common.MenuParent;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.UI
{
    public class SettingsMenuButtons : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _soundSettingsButton;
        [SerializeField] private Button _cameraSettingsButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private UIAnimatorSequence uIAnimatorSequence;

        public UnityEvent SoundSettingsPressed => _soundSettingsButton.onClick;
        public UnityEvent CameraSettingsPressed => _cameraSettingsButton.onClick;
        public UnityEvent BackPressed => _backButton.onClick;

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            gameObject.SetActive(true);
            uIAnimatorSequence.StartSequence();
        }
    }
}