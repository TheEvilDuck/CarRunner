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
            _button = GetComponent<Button>();
        }

        public void Lock() => _button.image.color = Color.red;

        public void MarkAsCompleted() => _button.image.color = Color.green;
    }
}
