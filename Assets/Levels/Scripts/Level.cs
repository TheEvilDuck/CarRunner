using System.Collections.Generic;
using Barmetler.RoadSystem;
using Gameplay;
using Gameplay.Garages;
using Gameplay.TimerGates;
using UnityEngine;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Garage> _garages;
        [SerializeField] private List<TimerGate> _timerGates;
        [SerializeField] private SimpleCarCollisionTrigger _finish;
        [SerializeField] private Transform _carStartPosition;
        [SerializeField] private RoadMeshGenerator _roadMeshGenerator;

        public IEnumerable<Garage> Garages => _garages;
        public IEnumerable<TimerGate> TimerGates => _timerGates;
        public SimpleCarCollisionTrigger Finish => _finish;
        public Vector3 CarStartPosition => _carStartPosition.position;

        private void Awake() 
        {
            _roadMeshGenerator.GenerateRoadMesh();
        }
    }
}