using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class PauseButton : MonoBehaviour
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
    }

}