using Common.MenuParent;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainButtons : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _play;
        [SerializeField] private Button _exit;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _shop;
        [SerializeField] private Button _tutorial;
        [SerializeField] private Button _language;
        [SerializeField] private Button _credits;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public UnityEvent PlayClickedEvent => _play.onClick;
        public UnityEvent ExitClickedEvent => _exit.onClick;
        public UnityEvent SettingsClickedEvent => _settings.onClick;
        public UnityEvent ShopClicked => _shop.onClick;
        public UnityEvent TutorialClicked => _tutorial.onClick;
        public UnityEvent LanguageClicked => _language.onClick;
        public UnityEvent CreditsClicked => _credits.onClick;

        public void Init(bool isWeb)
        {
            if (isWeb)
                _exit.gameObject.SetActive(false);
        }

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
        }
    }
}