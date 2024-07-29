using Common.MenuParent;
using Common.UI.UIAnimations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class TutorialView : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _understandableButton;
        [SerializeField] private Image _presentationSlide;
        [SerializeField] private List<Sprite> _tutorialImages;
        //[SerializeField] private UIAnimatorSequence _animations;
        private int _currentPresentationSlide = 0;

        public UnityEvent BackPressed => _backButton.onClick;
        public UnityEvent UnderstandablePressed => _understandableButton.onClick;

        private void OnEnable()
        {
            _presentationSlide.sprite = _tutorialImages[_currentPresentationSlide];

            _nextButton.onClick.AddListener(OnNextButtonPressed);
            _previousButton.onClick.AddListener(OnPreviousButtonPressed);
            _understandableButton.onClick.AddListener(OnUnderstandableButtonPressed);

            _understandableButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _nextButton.onClick.RemoveListener(OnNextButtonPressed);
            _previousButton.onClick.RemoveListener(OnPreviousButtonPressed);
            _understandableButton.onClick.RemoveListener(OnUnderstandableButtonPressed);
        }

        private void OnNextButtonPressed()
        {
            if(_currentPresentationSlide < _tutorialImages.Count - 1)
            {
                _currentPresentationSlide++;
                _presentationSlide.sprite = _tutorialImages[_currentPresentationSlide];

                if(_currentPresentationSlide == _tutorialImages.Count - 1)
                {
                    _nextButton.gameObject.SetActive(false);
                    _understandableButton.gameObject.SetActive(true);
                }
            }
        }

        private void OnPreviousButtonPressed()
        {
            if (_currentPresentationSlide > 0)
            {
                if(_currentPresentationSlide == _tutorialImages.Count - 1)
                {
                    _nextButton.gameObject.SetActive(true);
                    _understandableButton.gameObject.SetActive(false);
                }

                _currentPresentationSlide--;
                _presentationSlide.sprite = _tutorialImages[_currentPresentationSlide];
            }
        }

        private void OnUnderstandableButtonPressed()
        {

        }

        public void Hide() => gameObject.SetActive(false);

        public void Show()
        {
            gameObject.SetActive(true);
            //тут был старт анимации
        }
    }
}