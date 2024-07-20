using Common.MenuParent;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class PauseMainButtons : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public UnityEvent ResumeClicked => _resumeButton.onClick;
        public UnityEvent SettingsClicked => _settingsButton.onClick;

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
        }
    }

}