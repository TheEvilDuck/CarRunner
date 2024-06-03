using System;
using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class LevelSelector: MonoBehaviour
    {
        [SerializeField] private LevelsDatabase _levels;
        [SerializeField] private LevelButton _levelButtonPrefab;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private Button _backButton;

        public event Action<string> levelSelected;

        private Dictionary<LevelButton, string> _buttons;

        public UnityEvent BackPressed => _backButton.onClick;

        private void Awake() 
        {
            _buttons = new Dictionary<LevelButton, string>();

            foreach (string levelId in _levels.GetAllLevels())
            {
                LevelButton button = Instantiate(_levelButtonPrefab, _buttonsParent);
                button.Init(levelId);
                _buttons.Add(button, levelId);

                button.Clicked.AddListener(() => levelSelected.Invoke(levelId));
            }
        }
    }
}