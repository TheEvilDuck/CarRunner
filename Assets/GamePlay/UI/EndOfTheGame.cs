using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EndOfTheGame : MonoBehaviour
{
    [SerializeField] private Button _restart;
    [SerializeField] private Button _mainMenu;
    [SerializeField] private TextMeshProUGUI _endGameText;

    public UnityEvent RestartClickedEvent => _restart.onClick;
    public UnityEvent MainMenuClickedEvent => _mainMenu.onClick;

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    public void Win() => _endGameText.text = "Win";
    public void Lose() => _endGameText.text = "Lose";
}
