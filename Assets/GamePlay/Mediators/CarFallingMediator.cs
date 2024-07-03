using System;
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

        public CarFallingMediator(FallTries fallTries, FallingEndGame fallingEndGame, FallingBehaviourSwitcher fallingBehaviourSwitcher, CarFalling carFalling)
        {
            _fallTries = fallTries;
            _fallingEndGame = fallingEndGame;
            _fallingBehaviourSwitcher = fallingBehaviourSwitcher;
            _carFalling = carFalling;

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