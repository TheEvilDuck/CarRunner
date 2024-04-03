using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _play;
        [SerializeField] private Button _exit;

        public UnityEvent PlayClickedEvent => _play.onClick;
        public UnityEvent ExitClickedEvent => _exit.onClick;

    }
}

