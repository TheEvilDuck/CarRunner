using System;
using System.Collections;
using System.Collections.Generic;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class NotEnoughMoneyPopup : MonoBehaviour
    {
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public event Action yesClicked;
        public event Action noClicked;

        private void OnEnable() 
        {
            _yesButton.onClick.AddListener(OnYesButtonClicked);
            _noButton.onClick.AddListener(OnNoButtonClicked);
        }

        private void OnDisable() 
        {
            _yesButton.onClick.RemoveListener(OnYesButtonClicked);
            _noButton.onClick.RemoveListener(OnNoButtonClicked);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
        }

        public void Hide() => gameObject.SetActive(false);

        private void OnYesButtonClicked()
        {
            Hide();
            yesClicked?.Invoke();
        }

        private void OnNoButtonClicked()
        {
            Hide();
            noClicked?.Invoke();
        }
    }
}
