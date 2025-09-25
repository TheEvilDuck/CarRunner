using System.Linq;
using Levels.Scripts;
using Services.PlayerData;
using UnityEngine;

namespace Services.RewardProvider
{
    public class RewardProvider: IRewardProvider
    {
        private const int TUTORIAL_COMPLETION_REWARD = 2000;
        private const float COINS_MULTIPLIER_FOR_REPLAYING_LEVEL = 0.5f;
        
        private readonly LevelsDatabase _levelsDatabase;
        private readonly IPlayerData _playerData;

        public RewardProvider(
            IPlayerData playerData, 
            LevelsDatabase levelsDatabase)
        {
            _playerData = playerData;
            _levelsDatabase = levelsDatabase;
        }

        public int GetLevelCompletionReward(float remainingTime, string levelID)
        {
            var level = _levelsDatabase.GetLevel(_playerData.SelectedLevel);
            float startTime = level.StartTimer;
            float sumOfTimerGates = 0;
            int maxReward = _levelsDatabase.GetMaxReward(levelID);

            foreach (var timerGate in level.TimerGates)
                if (timerGate.Time > 0)
                    sumOfTimerGates += timerGate.Time;

            float k = 1f;

            if (_playerData.PassedLevels.Contains(levelID))
                k = COINS_MULTIPLIER_FOR_REPLAYING_LEVEL;
            
            int coins = Mathf.CeilToInt(maxReward * (remainingTime / (startTime + sumOfTimerGates)) * k);
            return coins;
        }

        public int GetTutorialCompletionReward() => TUTORIAL_COMPLETION_REWARD;
    }

}