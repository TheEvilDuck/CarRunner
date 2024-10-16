using Common.MenuParent;
using Common.UI.UIAnimations;
using Services.Localization;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class TutorialView : MonoBehaviour, IMenuParent, ILocalizable
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _understandableButton;
        [SerializeField] private UIPositionAnimator _posAnimNextButton;
        [SerializeField] private UITransparancyAnimator _transAnimNextButton;
        [SerializeField] private UIPositionAnimator _posAnimUnderstandButton;
        [SerializeField] private UITransparancyAnimator _transAnimUnderstandButton;
        [SerializeField] private Image _presentationSlide;
        [SerializeField] private List<TutorialData> _tutorialData;
        [SerializeField] private UIAnimatorSequence _animations;
        [SerializeField] private TextMeshProUGUI _slideText;
        private List<TutorialDataElement> _tutorialDataElements;
        private int _currentPresentationSlide = 0;

        public event Action<ILocalizable> updateRequested;

        public UnityEvent BackPressed => _backButton.onClick;
        public UnityEvent UnderstandablePressed => _understandableButton.onClick;

        public string TextId {get; private set;}

        public void Init(DeviceType deviceType)
        {
            foreach(TutorialData tutorialData in _tutorialData)
            {
                if (tutorialData.DeviceType == deviceType)
                    _tutorialDataElements = tutorialData.TutorialDataElements;
            }

            TextId = _tutorialDataElements[_currentPresentationSlide].TextId;
            LocalizationRegistrator.Instance.RegisterLocalizable(this, true);

            
        }

        private void OnEnable()
        {
            _nextButton.onClick.AddListener(OnNextButtonPressed);
            _previousButton.onClick.AddListener(OnPreviousButtonPressed);

            _currentPresentationSlide = 0;
            _presentationSlide.sprite = _tutorialDataElements[_currentPresentationSlide].Sprite;
            TextId = _tutorialDataElements[_currentPresentationSlide].TextId;
            updateRequested?.Invoke(this);
            _understandableButton.gameObject.SetActive(false);

            _animations.StartSequence();
        }

        private void OnDisable()
        {
            _nextButton.onClick.RemoveListener(OnNextButtonPressed);
            _previousButton.onClick.RemoveListener(OnPreviousButtonPressed);

            _currentPresentationSlide = 0;
            _nextButton.gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        private void OnNextButtonPressed()
        {
            if(_currentPresentationSlide < _tutorialDataElements.Count - 1)
            {
                _currentPresentationSlide++;
                _presentationSlide.sprite = _tutorialDataElements[_currentPresentationSlide].Sprite;

                if(_currentPresentationSlide == _tutorialDataElements.Count - 1)
                {
                    _nextButton.gameObject.SetActive(false);
                    _understandableButton.gameObject.SetActive(true);
                    _posAnimUnderstandButton.StartAnimation();
                    _transAnimUnderstandButton.StartAnimation();
                }

                TextId = _tutorialDataElements[_currentPresentationSlide].TextId;
                updateRequested?.Invoke(this);
            }
        }

        private void OnPreviousButtonPressed()
        {
            if (_currentPresentationSlide > 0)
            {
                if(_currentPresentationSlide == _tutorialDataElements.Count - 1)
                {
                    _nextButton.gameObject.SetActive(true);
                    _posAnimNextButton.StartAnimation();
                    _transAnimNextButton.StartAnimation();
                    _understandableButton.gameObject.SetActive(false);
                }

                _currentPresentationSlide--;
                _presentationSlide.sprite = _tutorialDataElements[_currentPresentationSlide].Sprite;

                TextId = _tutorialDataElements[_currentPresentationSlide].TextId;
                updateRequested?.Invoke(this);
            }
        }

        private void OnValidate()
        {
            for(int i = 0; i < _tutorialData.Count; i++)
            {
                if(i < _tutorialData.Count - 1)
                {
                    for (int j = i + 1; j < _tutorialData.Count; j++)
                    {
                        if (_tutorialData[i].DeviceType == _tutorialData[j].DeviceType)
                            throw new ArgumentException($"You have 2 sets of slides for the same platform: {_tutorialData[i].DeviceType}");
                    }
                }
            }
        }

        public void UpdateText(string text)
        {
            _slideText.text = text;
        }

        [Serializable]
        private class TutorialData
        {
            [field: SerializeField] public DeviceType DeviceType { get; private set; }
            [field: SerializeField] public List<TutorialDataElement> TutorialDataElements { get; private set; }
        }

        [Serializable]
        private class TutorialDataElement
        {
            [field: SerializeField] public Sprite Sprite { get; private set; }
            [field: SerializeField] public string TextId { get; private set; }
        }
    }
}