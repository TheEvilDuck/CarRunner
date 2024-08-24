using Common.Sound;
using Common.States;
using DI;
using Gameplay.CarFallingHandling;
using Gameplay.Cars;
using Levels;

namespace Gameplay.States
{
    public class RaceGameState : State
    {
        private readonly Timer _timer;
        private readonly CarBehaviour _car;
        private readonly SimpleCarCollisionTrigger _finish;
        private readonly SoundController _soundController;
        private readonly Level _level;
        private readonly FallingEndGame _fallingEndGame;
        public RaceGameState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _timer = sceneContext.Get<Timer>();
            _car = sceneContext.Get<Car>().CarBehavior;
            _level = sceneContext.Get<Level>();
            _finish = _level.Finish;
            _fallingEndGame = sceneContext.Get<FallingEndGame>();
            _soundController = sceneContext.Get<SoundController>();
        }

        public override void Update()
        {
            _timer.Update();
        }

        protected override void OnEnter()
        {
            _timer.Restart();
            _car.enabled = true;
            _soundController.Play(_level.BackGroundMusicId);

            _timer.end+=Lose;
            _fallingEndGame.falled += Lose;
            _finish.passed += Win;
        }
        protected override void OnExit()
        {
            _timer.end-=Lose;
            _finish.passed -= Win;
            _fallingEndGame.falled -= Lose;
        }

        private void Lose()
        {
            _stateMachine.ChangeState<LoseState>();
        }

        private void Win()
        {
            _stateMachine.ChangeState<WinState>();
        }
    }
}
