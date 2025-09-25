using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GamePlay.UI.Scripts
{
    public class SceneChangingButtons : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _goToMainMenuButton;

        public UnityEvent RestartButtonPressed => _restartButton.onClick;
        public UnityEvent GoToMainMenuButtonPressed => _goToMainMenuButton.onClick;
    }
}
