using Common.UI.UIAnimations;
using TMPro;
using UnityEngine;

public class EndOfTheGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _endGameText;
    [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;
    [field: SerializeField] public SceneChangingButtons SceneChangingButtons;

    private void OnEnable() => _uIAnimatorSequence.StartSequence();

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    public void Win(int reward) => _endGameText.text = $"Win! You earned: {reward}";
    public void Lose() => _endGameText.text = "Lose";
}
