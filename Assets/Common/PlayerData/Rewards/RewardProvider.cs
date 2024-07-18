using Levels;
using UnityEngine;

namespace Common.Data.Rewards
{
    public class RewardProvider
    {
        public int GetLevelCompletionReward(float remainingTime, IPlayerData playerData, LevelsDatabase levelsDatabase)
        {
            string nextLevelId = levelsDatabase.GetNextLevelId(playerData.SelectedLevel);
            int nextLevelCost = levelsDatabase.GetLevelCost(nextLevelId);
            var level = levelsDatabase.GetLevel(playerData.SelectedLevel);
            float startTime = level.StartTimer;
            float sumOfTimerGates = 0;
            float rewardDivider = levelsDatabase.GetLevelRewardDivider(playerData.SelectedLevel);

            foreach (var timerGate in level.TimerGates)
                if (timerGate.Time > 0)
                    sumOfTimerGates += timerGate.Time;
            
            int coins = Mathf.CeilToInt(nextLevelCost * (remainingTime / (startTime + sumOfTimerGates)) * rewardDivider);
            return coins;
        }
    }

}