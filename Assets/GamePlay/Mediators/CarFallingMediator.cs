using System;
using DI;
using Gameplay.CarFallingHandling;
using UnityEngine;

namespace Gameplay
{
    public class CarFallingMediator : IDisposable
    {
        private readonly FallTries _fallTries;
        private readonly FallingEndGame _fallingEndGame;
        private readonly CarFalling _carFalling;
        private readonly FallingBehaviourSwitcher _fallingBehaviourSwitcher;

        public CarFallingMediator(DIContainer sceneContext)
        {
            _fallTries = sceneContext.Get<FallTries>();
            _fallingEndGame = sceneContext.Get<FallingEndGame>();
            _fallingBehaviourSwitcher = sceneContext.Get<FallingBehaviourSwitcher>();
            _carFalling = sceneContext.Get<CarFalling>();

            _fallTries.triesEnd += OnFallTriesEnd;
            _carFalling.carFallen += OnCarFallen;
        }
        public void Dispose()
        {
            _fallTries.triesEnd -= OnFallTriesEnd;
            _carFalling.carFallen -= OnCarFallen;
        }

        private void OnFallTriesEnd()
        {
            _fallingBehaviourSwitcher.AttachBehaviour(_fallingEndGame);
        }

        private void OnCarFallen(Vector3 position, Quaternion rotation)
        {
            _fallTries.SpendTry();
        }
    }

}