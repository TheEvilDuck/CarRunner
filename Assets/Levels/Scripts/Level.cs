using System.Collections.Generic;
using Gameplay;
using Gameplay.Garages;
using Gameplay.TimerGates;
using RoadArchitect;
using UnityEngine;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Garage> _garages;
        [SerializeField] private List<TimerGate> _timerGates;
        [SerializeField] private SimpleCarCollisionTrigger _finish;
        [SerializeField] private Transform _carStartPosition;
        [SerializeField] private RoadSystem _roadSystem;
        [field: SerializeField, Min(0)] public float StartTimer;

        public IEnumerable<Garage> Garages => _garages;
        public IEnumerable<TimerGate> TimerGates => _timerGates;
        public SimpleCarCollisionTrigger Finish => _finish;
        public Vector3 CarStartPosition => _carStartPosition.position;

        public void Init()
        {
            _roadSystem?.UpdateAllRoads();
        }
    }
}
