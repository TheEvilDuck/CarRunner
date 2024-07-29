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
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public UnityEvent PlayClickedEvent => _play.onClick;
        public UnityEvent ExitClickedEvent => _exit.onClick;
        public UnityEvent SettingsClickedEvent => _settings.onClick;
        public UnityEvent ShopClicked => _shop.onClick;
        public UnityEvent TutorialClicked => _tutorial.onClick;

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
        }
    }

}