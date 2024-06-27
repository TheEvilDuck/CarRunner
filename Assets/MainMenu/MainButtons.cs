using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainButtons : MonoBehaviour
    {
        [SerializeField] private Button _play;
        [SerializeField] private Button _exit;
        [SerializeField] private Button _settings;

        public UnityEvent PlayClickedEvent => _play.onClick;
        public UnityEvent ExitClickedEvent => _exit.onClick;
        public UnityEvent SettingsClickedEvent => _settings.onClick;
    }

}