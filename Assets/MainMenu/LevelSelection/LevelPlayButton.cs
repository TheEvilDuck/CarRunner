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

        public UnityEvent clicked => _button.onClick;

        public string TextId {get; private set;}

        private void Start() 
        {
            LocalizationRegistrator.Instance.RegisterLocalizable(this);
        }

        private void OnEnable() => _uIAnimatorSequence.StartSequence();
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        public void ShowPlayText()
        {
            TextId = _playTextId;
            updateRequested?.Invoke(this);
        }
        public void ShowLocked(int cost) => _buttonText.text = cost.ToString();

        public void UpdateText(string text)
        {
            _buttonText.text = text;
        }
    }
}
