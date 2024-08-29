using System;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class PauseButton : MonoBehaviour, IPausable
    {
        [SerializeField] private Button _button;
        public event Action pressed;

        private void OnEnable() 
        {
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnDisable() 
        {
            _button.onClick.RemoveListener(OnButtonPressed);
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        private void OnButtonPressed() => pressed?.Invoke();

        public void Pause() => Hide();

        public void Resume() => Show();
    }

}