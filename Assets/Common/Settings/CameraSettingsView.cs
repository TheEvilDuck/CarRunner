using Common.MenuParent;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.UI
{
    public class CameraSettingsView : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Slider _angleOfView;
        [SerializeField] private Slider _zOffset;
        [SerializeField] private Button _backButton;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public UnityEvent<float> AngleOfViewChanged => _angleOfView.onValueChanged;
        public UnityEvent<float> ZOffsetChanged => _zOffset.onValueChanged;
        public UnityEvent BackPressed => _backButton.onClick;

        public void Init(GameSettings gameSettings)
        {
            _angleOfView.value = gameSettings.AngleOfView;
            _zOffset.value = gameSettings.ZOffset;
        }

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
        }
    }
}