using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _play;
        [SerializeField] private Button _exit;

        public UnityEvent PlayClickedEvent { get; }
        public UnityEvent ExitClickedEvent { get; }

        private void Awake()
        {
            _play.onClick.AddListener(OnPlayClecked);
            _exit.onClick.AddListener(OnExitClecked);
        }

        private void OnDisable()
        {
            _play.onClick.RemoveListener(OnPlayClecked);
            _exit.onClick.RemoveListener(OnExitClecked);
        }

        private void OnPlayClecked() => PlayClickedEvent?.Invoke();
        private void OnExitClecked() => ExitClickedEvent?.Invoke();

    }
}

