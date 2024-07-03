using System;
using UnityEngine;

namespace Gameplay.CarFallingHandling
{
    public class FallTries
    {
        public event Action<int> triesChanged;
        public event Action triesEnd;
        private readonly int _maxTries;
        private int _currentTries;

        public int MaxTries => _maxTries;
        public int CurrentTries => _currentTries;

        public FallTries(int maxTries)
        {
            _currentTries = _maxTries = maxTries;
        }

        public void SpendTry()
        {
            if (_currentTries == 0)
                return;

            _currentTries -= 1;
            triesChanged?.Invoke(_currentTries);

            if (_currentTries == 0)
                triesEnd?.Invoke();
        }
    }

}