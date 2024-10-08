using System;
using System.Collections.Generic;
using Common.MenuParent;
using Common.UI.UIAnimations;
using Services.Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.LanguageSelection
{
    public class LanguageSelectorMenu : MonoBehaviour, IMenuParent
    {
        [SerializeField] private LanguageUI _languageUIPrefab;
        [SerializeField] private Transform _languageUIParent;
        [SerializeField] private Button _backButton;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public event Action<string> languageChanged;

        private Dictionary<LanguageUI, UnityAction> _buttons;
        private LanguageUI _currentSelected;

        public UnityEvent BackPressed => _backButton.onClick;
        public void Init(IEnumerable<LanguageData> languageData, string currentLanguage)
        {
            _buttons = new Dictionary<LanguageUI, UnityAction>();

            foreach (var data in languageData)
            {
                var ui = Instantiate(_languageUIPrefab, _languageUIParent);
                ui.Init(data.LanguageNativeName, data.LangiageIcon);

                if (data.LanguageId == currentLanguage)
                {
                    ui.Select();
                    _currentSelected = ui;
                }
                else
                    ui.Unselect();

                UnityAction onPressed = new UnityAction(() => 
                {
                    languageChanged?.Invoke(data.LanguageId);

                    if (_currentSelected != null)
                        _currentSelected.Unselect();

                    _currentSelected = ui;
                    _currentSelected.Select();

                    Debug.Log($"trying to set language to {data.LanguageId}");
                });

                ui.Pressed.AddListener(onPressed);

                _uIAnimatorSequence.AddAnimation(ui.UITransparancyAnimator, 0.1f, false);

                _buttons.Add(ui, onPressed);
            }
        }

        private void OnDestroy() 
        {
            foreach (var pair in _buttons)
                if (pair.Key != null)
                    pair.Key.Pressed.RemoveListener(pair.Value);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
        }
    }
}
