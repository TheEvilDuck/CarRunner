using System;
using Common.UI.UIAnimations;
using Gameplay.UI;
using Services.Localization;
using TMPro;
using UnityEngine;

public class EndOfTheGame : MonoBehaviour, ILocalizable
{
    [SerializeField] private TextMeshProUGUI _endGameText;
    [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;
    [SerializeField] private UINumberTextAnimator _winTextAnimation;
    [field: SerializeField] public AdButton AdButton;
    [SerializeField] private string _winTextId;
    [SerializeField] private string _lostTextId;

    public event Action<ILocalizable> updateRequested;

    [field: SerializeField] public SceneChangingButtons SceneChangingButtons {get; private set;}

    public string TextId {get; private set;}

    private void Start() 
    {
        LocalizationRegistrator.Instance.RegisterLocalizable(this);
    }

    private void OnEnable() => _uIAnimatorSequence.StartSequence();

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    public void Win(int reward)
    {
        _winTextAnimation.gameObject.SetActive(true);
        _winTextAnimation.enabled = true;
        _winTextAnimation.ChangeTargetValue(0, reward);
        AdButton.Show();
        TextId = _winTextId;
        updateRequested?.Invoke(this);
    }
    public void Lose()
    {
        _winTextAnimation.enabled = false;
        _winTextAnimation.gameObject.SetActive(false);
        AdButton.Hide();
        TextId = _lostTextId;
        updateRequested?.Invoke(this);
    }

    public void UpdateText(string text)
    {
        _endGameText.text = text;
    }
}
