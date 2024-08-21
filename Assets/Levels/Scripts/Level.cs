using System.Collections.Generic;
using Common.Sound;
using Gameplay;
using Gameplay.Cars;
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
        [SerializeField] private CarConfig _startCar;
        [SerializeField] private Material _skybox;
        [field: SerializeField, Min(0)] public float StartTimer;
        [field: SerializeField] public SoundID BackGroundMusicId;

        public IEnumerable<Garage> Garages => _garages;
        public IEnumerable<TimerGate> TimerGates => _timerGates;
        public SimpleCarCollisionTrigger Finish => _finish;
        public Vector3 CarStartPosition => _carStartPosition.position;
        public Quaternion CarStartRotation => _carStartPosition.rotation;
        public CarConfig StartCar => _startCar;
        public Material Skybox => _skybox;
    }
}
