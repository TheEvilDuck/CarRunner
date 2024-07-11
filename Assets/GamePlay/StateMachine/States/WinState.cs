using Common;
using Common.Data;
using Common.States;
using DI;
using Levels;
using UnityEngine;

namespace Gameplay.States
{
    public class WinState : GameOverState
    {
        public WinState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine, sceneContext)
        {
        }

        protected override void OnEnter()
        {
            base.OnEnter();

            var playerData = _sceneContext.Get<IPlayerData>();
            var levelDatabase = _sceneContext.Get<LevelsDatabase>();
            var timer = _sceneContext.Get<Timer>();
            playerData.AddPassedLevel(playerData.SelectedLevel);

            var nextLevelId = levelDatabase.GetNextLevelId(playerData.SelectedLevel);
            int nextLevelCost = levelDatabase.GetLevelCost(nextLevelId);
            var level = levelDatabase.GetLevel(playerData.SelectedLevel);
            float startTime = level.StartTimer;
            float sumOfTimerGates = 0;
            float rewardDivider = levelDatabase.GetLevelRewardDivider(playerData.SelectedLevel);

            foreach (var timerGate in level.TimerGates)
                if (timerGate.Time > 0)
                    sumOfTimerGates += timerGate.Time;
            
            int coins = Mathf.CeilToInt(nextLevelCost * (timer.CurrentTime / (startTime + sumOfTimerGates)) * rewardDivider);
            playerData.AddCoins(coins);

            _sceneContext.Get<EndOfTheGame>().Win(coins);
        }
    }
}
