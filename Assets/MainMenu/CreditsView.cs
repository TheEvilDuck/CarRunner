using Common.MenuParent;
using Common.UI.UIAnimations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class CreditsView : MonoBehaviour, IMenuParent
    {
        [SerializeField] private TextMeshProUGUI _credits;
        [SerializeField] private Button _back;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public UnityEvent BackPressed => _back.onClick;

        public void Show()
        {
            gameObject.SetActive(true);
            _credits.gameObject.SetActive(true);
            _back.gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
        }

        public void Hide() => gameObject.SetActive(false);
    }
}