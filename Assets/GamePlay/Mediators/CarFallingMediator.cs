using System;
using DI;
using Gameplay.CarFallingHandling;
using Levels;
using UnityEngine;

namespace Gameplay
{
    public class CarFallingMediator : IDisposable
    {
        private readonly FallTries _fallTries;
        private readonly FallingEndGame _fallingEndGame;
        private readonly FallingTeleport _fallingTeleport;
        private readonly CarFalling _carFalling;
        private readonly FallingBehaviourSwitcher _fallingBehaviourSwitcher;
        private readonly Level _level;

        public CarFallingMediator(DIContainer sceneContext)
        {
            _fallTries = sceneContext.Get<FallTries>();
            _fallingEndGame = sceneContext.Get<FallingEndGame>();
            _fallingBehaviourSwitcher = sceneContext.Get<FallingBehaviourSwitcher>();
            _carFalling = sceneContext.Get<CarFalling>();
            _level = sceneContext.Get<Level>();
            _fallingTeleport = sceneContext.Get<FallingTeleport>();

            _fallTries.triesEnd += OnFallTriesEnd;
            _carFalling.carFallen += OnCarFallen;
            _level.Finish.passed += OnFinish;
        }
        public void Dispose()
        {
            _fallTries.triesEnd -= OnFallTriesEnd;
            _carFalling.carFallen -= OnCarFallen;
            _level.Finish.passed -= OnFinish;
        }

        private void OnFallTriesEnd()
        {
            _fallingBehaviourSwitcher.AttachBehaviour(_fallingEndGame);
        }

        private void OnCarFallen(Vector3 position, Quaternion rotation)
        {
            _fallTries.SpendTry();
        }

        private void OnFinish()
        {
            _carFalling.carFallen -= OnCarFallen;
            _fallingBehaviourSwitcher.AttachBehaviour(_fallingTeleport);
        }
    }

}