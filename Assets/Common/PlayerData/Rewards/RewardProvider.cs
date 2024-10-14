using System.Linq;
using Levels;
using UnityEngine;

namespace Common.Data.Rewards
{
    public class RewardProvider
    {
        private const int TUTORIAL_COMPLETION_REWARD = 2000;

        private const float COINS_MULTIPLIER_FOR_REPLAYING_LEVEL = 0.5f;
        public int GetLevelCompletionReward(float remainingTime, IPlayerData playerData, LevelsDatabase levelsDatabase)
        {
            var level = levelsDatabase.GetLevel(playerData.SelectedLevel);
            float startTime = level.StartTimer;
            float sumOfTimerGates = 0;
            int maxReward = levelsDatabase.GetMaxReward(playerData.SelectedLevel);

            foreach (var timerGate in level.TimerGates)
                if (timerGate.Time > 0)
                    sumOfTimerGates += timerGate.Time;

            float k = 1f;

            if (playerData.PassedLevels.Contains(playerData.SelectedLevel))
                k = COINS_MULTIPLIER_FOR_REPLAYING_LEVEL;
            
            int coins = Mathf.CeilToInt(maxReward * (remainingTime / (startTime + sumOfTimerGates)) * k);
            return coins;
        }

        public int GetTutorialCompletionReward() => TUTORIAL_COMPLETION_REWARD;
    }

}