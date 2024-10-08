using System;
using Common.UI.UIAnimations;
using Services.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.LevelSelection
{
    public class LevelPlayButton : MonoBehaviour, ILocalizable
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;
        [SerializeField] private string _playTextId;

        public event Action<ILocalizable> updateRequested;

        private bool _registered;

        public UnityEvent clicked => _button.onClick;
        public string TextId {get; private set;}

        private void OnEnable()
        {
            _button.enabled = false;
            _uIAnimatorSequence.end += OnAnimationEnd;
            _uIAnimatorSequence.StartSequence();
        }

        private void OnDisable() 
        {
            _uIAnimatorSequence.end -= OnAnimationEnd;
        }
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        public void ShowPlayText()
        {
            if (!_registered)
            {
                LocalizationRegistrator.Instance.RegisterLocalizable(this, false);
                _registered = true;
            }

            TextId = _playTextId;
            updateRequested?.Invoke(this);
        }
        public void ShowLocked(int cost) => _buttonText.text = cost.ToString();

        public void UpdateText(string text)
        {
            _buttonText.text = text;
        }

        private void OnAnimationEnd()
        {
            _button.enabled = true;
        }
    }
}
