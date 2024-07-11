using Common.UI.UIAnimations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.LevelSelection
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private UIPositionAnimator _uIPositionAnimator;
        [SerializeField] private UITransparancyAnimator _uITransparancyAnimator;
        [SerializeField] private Button _button;

        private Color _unlockColor;

        public UnityEvent Clicked => _button.onClick;
        public UIAnimator PosittionAnimator => _uIPositionAnimator;
        public UIAnimator TransparencyAnimation => _uITransparancyAnimator;

        public void Init(string name)
        {
            _nameText.text = name;
            _unlockColor = _button.image.color;
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
    }
}
