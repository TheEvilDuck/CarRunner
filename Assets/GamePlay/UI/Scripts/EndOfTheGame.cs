using TMPro;
using UnityEngine;

public class EndOfTheGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _endGameText;
    [field: SerializeField] public SceneChangingButtons SceneChangingButtons;

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    public void Win() => _endGameText.text = "Win";
    public void Lose() => _endGameText.text = "Lose";
}
