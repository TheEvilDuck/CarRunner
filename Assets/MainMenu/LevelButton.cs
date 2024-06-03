using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        private Button _button;

        public UnityEvent Clicked => _button.onClick;

        public void Init(string name)
        {
            _nameText.text = name;
        }

        private void Awake() 
        {
            _button = GetComponent<Button>();
        }
    }
}
