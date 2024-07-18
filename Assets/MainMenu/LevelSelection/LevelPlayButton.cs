using Common.UI.UIAnimations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.LevelSelection
{
    public class LevelPlayButton : MonoBehaviour
    {
        private const string PLAY_TEXT = "Play";
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public UnityEvent clicked => _button.onClick;

        private void OnEnable() => _uIAnimatorSequence.StartSequence();
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        public void ShowPlayText() => _buttonText.text = PLAY_TEXT;
        public void ShowLocked(int cost) => _buttonText.text = cost.ToString();
    }
}
