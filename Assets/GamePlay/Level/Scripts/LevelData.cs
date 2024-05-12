using System.Collections.Generic;
using Gameplay.Garages;
using Gameplay.TimerGates;
using UnityEngine;

namespace Gameplay.Levels
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField] private List<Garage> _garages;
        [SerializeField] private List<TimerGate> _timerGates;
        [SerializeField] private SimpleCarCollisionTrigger _finish;
        [SerializeField] private Transform _carStartPosition;

        public IEnumerable<Garage> Garages => _garages;
        public IEnumerable<TimerGate> TimerGates => _timerGates;
        public SimpleCarCollisionTrigger Finish => _finish;
        public Vector3 CarStartPosition => _carStartPosition.position;
    }
}
