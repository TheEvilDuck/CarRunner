using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.LanguageSelection
{
    public class LanguageUI : MonoBehaviour
    {
        [SerializeField] private Image _languageIcon;
        [SerializeField] private TextMeshProUGUI _languageText;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _selectionIndicator;

        public UnityEvent Pressed => _button.onClick;

        public void Init(string languageText, Sprite languageIcon)
        {
            _languageIcon.sprite = languageIcon;
            _languageText.text = languageText;
        }

        public void Select() => _selectionIndicator.SetActive(true);

        public void Unselect() => _selectionIndicator.SetActive(false);
    }
}
