using System;
using Common.UI.UIAnimations;
using Services.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.LevelSelection
{
    public class LevelButton : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private UIPositionAnimator _uIPositionAnimator;
        [SerializeField] private UITransparancyAnimator _uITransparancyAnimator;
        [SerializeField] private Button _button;

        private Color _unlockColor;

        public event Action<ILocalizable> updateRequested;

        public UnityEvent Clicked => _button.onClick;
        public UIAnimator PosittionAnimator => _uIPositionAnimator;
        public UIAnimator TransparencyAnimation => _uITransparancyAnimator;

        public string TextId {get; private set;}

        public void Init(string nameId)
        {
            TextId = nameId;
            _unlockColor = _button.image.color;
            LocalizationRegistrator.Instance.RegisterLocalizable(this);
        }

        public void Unlock()
        {
            _button.image.color = _unlockColor;
        }

        public void Lock() 
        {
            _button.image.color = Color.red;
        } 

        public void MarkAsCompleted() => _button.image.color = Color.green;

        public void UpdateText(string text)
        {
            _nameText.text = text;
        }
    }
}
