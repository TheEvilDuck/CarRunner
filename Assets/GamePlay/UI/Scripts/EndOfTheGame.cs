using Common.UI.UIAnimations;
using Gameplay.UI;
using TMPro;
using UnityEngine;

public class EndOfTheGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _endGameText;
    [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;
    [SerializeField] private UINumberTextAnimator winTextAnimation;
    [SerializeField] private AdButton _adButton;
    [field: SerializeField] public SceneChangingButtons SceneChangingButtons {get; private set;}

    private void OnEnable() => _uIAnimatorSequence.StartSequence();

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    public void Win(int reward)
    {
        winTextAnimation.enabled = true;
        winTextAnimation.ChangeTargetValue(reward);
        _adButton.Show();
    }
    public void Lose()
    {
        winTextAnimation.enabled = false;
        _adButton.Hide();
        _endGameText.text = "Lose";
    }
}
