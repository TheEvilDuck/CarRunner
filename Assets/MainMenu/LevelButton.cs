using System;
using Common.UI.UIAnimations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private UIPositionAnimator _uIPositionAnimator;
        [SerializeField] private UITransparancyAnimator _uITransparancyAnimator;
        [SerializeField] private Button _button;

        public UnityEvent Clicked => _button.onClick;
        public UIAnimator PosittionAnimator => _uIPositionAnimator;
        public UIAnimator TransparencyAnimation => _uITransparancyAnimator;

        public void Init(string name)
        {
            _nameText.text = name;
        }

        public void Lock() 
        {
            _button.interactable = false;
            _button.image.color = Color.red;
        } 

        public void MarkAsCompleted() => _button.image.color = Color.green;
        }
    }
}
