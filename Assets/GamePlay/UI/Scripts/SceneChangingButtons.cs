using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SceneChangingButtons : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _goToMainMenuButton;

    public UnityEvent RestartButtonPressed => _restartButton.onClick;
    public UnityEvent GoToMainMenuButtonPressed => _goToMainMenuButton.onClick;

    public void Show()
    {
        gameObject.SetActive(true);
        _restartButton.interactable = true;
        _goToMainMenuButton.interactable = true;
    }
    public void Hide()
    {
        _restartButton.interactable = false;
        _goToMainMenuButton.interactable = false;
        gameObject.SetActive(false);
    }
}
