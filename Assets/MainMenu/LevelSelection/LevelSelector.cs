using System;
using System.Collections.Generic;
using System.Linq;
using Common.Data;
using Common.MenuParent;
using Common.UI.UIAnimations;
using Levels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

namespace MainMenu.LevelSelection
{
    public class LevelSelector: MonoBehaviour, IMenuParent
    {
        [SerializeField] private LevelsDatabase _levels;
        [SerializeField] private LevelButton _levelButtonPrefab;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private Button _backButton;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;
        [SerializeField] private LevelPlayButton _levelPlayButton;
        [SerializeField] private LeaderboardYG _leaderboardYG;
        [SerializeField] private UIAnimatorSequence _leaderboardLoadingAnimation;
        [SerializeField] private GameObject _leaderboardLoadingGameObject;

        public event Action<string> levelSelected;
        public event Func<string, bool> buyLevelPressed;

        private Dictionary<LevelButton, string> _buttons;
        private string _currentLevelId;
        private bool _isReadyToPlay;

        public UnityEvent BackPressed => _backButton.onClick;

        public void Init(IEnumerable<string> passedLevels, IEnumerable<string> availableLevels, ILeaderBoardData leaderBoardData)
        {
            _buttons = new Dictionary<LevelButton, string>();
            _levelPlayButton.Hide();
            _leaderboardLoadingAnimation.gameObject.SetActive(false);

            foreach (string levelId in _levels.GetAllLevels())
            {
                LevelButton button = Instantiate(_levelButtonPrefab, _buttonsParent);
                button.Init(levelId);
                _buttons.Add(button, levelId);

                button.Clicked.AddListener(async () => 
                {
                    _levelPlayButton.Show();
                    _leaderboardYG.gameObject.SetActive(true);

                    _isReadyToPlay = availableLevels.Contains(levelId);

                    if(_isReadyToPlay)
                        _levelPlayButton.ShowPlayText();
                    else
                        _levelPlayButton.ShowLocked(_levels.GetLevelCost(levelId));

                    if (!string.Equals(_currentLevelId, levelId))
                    {
                        _currentLevelId = levelId;
                        _leaderboardYG.SetNameLB(YandexCloudLeaderboard.LEADERBOARD_KEY + _currentLevelId);
                        _leaderboardLoadingGameObject.SetActive(true);
                        _leaderboardLoadingAnimation.StartSequence();
                        var data = await leaderBoardData.GetLeaderBoard(levelId);
                        _leaderboardLoadingAnimation.StopSequence();
                        _leaderboardLoadingGameObject.SetActive(false);

                        if (data == null)
                        {
                            _leaderboardYG.ShowNoData();
                            return;
                        }

                        _leaderboardYG.UpdateLB(data);
                    }
                });

                _uIAnimatorSequence.AddAnimation(button.PosittionAnimator, 0.1f, false);
                _uIAnimatorSequence.AddAnimation(button.TransparencyAnimation, 0, false);
            }

            _levelPlayButton.clicked.AddListener(() => 
            {
                if (_isReadyToPlay)
                {
                    levelSelected?.Invoke(_currentLevelId);
                }
                else if (buyLevelPressed.Invoke(_currentLevelId))
                {
                    _levelPlayButton.ShowPlayText();
                    _isReadyToPlay = true;
                }

            });

            UpdateButtons(passedLevels, availableLevels);
        }

        public void UpdateButtons(IEnumerable<string> passedLevels, IEnumerable<string> availableLevels)
        {
            foreach (var buttonAndLevelId in _buttons)
            {
                if (passedLevels.Contains(buttonAndLevelId.Value))
                    buttonAndLevelId.Key.MarkAsCompleted();
                else if(!availableLevels.Contains(buttonAndLevelId.Value))
                    buttonAndLevelId.Key.Lock();
                else
                    buttonAndLevelId.Key.Unlock();
            }
        }
        public void Show()
        {
            gameObject.SetActive(true);
            _uIAnimatorSequence.StartSequence();
            _levelPlayButton.Hide();
            _leaderboardYG.gameObject.SetActive(false);
        }

        public void Hide()
        {
            _levelPlayButton.Hide();
            _leaderboardYG.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}