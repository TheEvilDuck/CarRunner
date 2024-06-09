using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Gameplay.Cars
{
    public class CarView : MonoBehaviour, IPausable
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        private Dictionary<Transform,IReadOnlyWheel> _currentWheels;
        private bool _paused = false;
        public void ChangeModel(Mesh mesh, Material[] materials)
        {
            _meshFilter.mesh = mesh;
            _meshRenderer.materials = materials;
        }

        public void InitWheels(GameObject _wheelPrefab, IEnumerable<IReadOnlyWheel> wheels)
        {
            CleanUp();

            _currentWheels = new Dictionary<Transform, IReadOnlyWheel>();

            foreach (IReadOnlyWheel wheel in wheels)
            {
                GameObject wheelView = Instantiate<GameObject>(_wheelPrefab);
                
                wheelView.transform.position = wheel.WorldPosition;
                wheelView.transform.rotation = wheel.WorldRotation;
                wheelView.transform.SetParent(transform);
                wheelView.transform.localScale = new Vector3(100f * wheel.Radius, 100f * wheel.Radius, 100f * wheel.Radius);

                _currentWheels.Add(wheelView.transform, wheel);
                
            }
        }

        public void Pause() => _paused = true;

        public void Resume() => _paused = false;

        private void CleanUp()
        {
            if (_currentWheels == null)
                return;

            foreach (var transformAndWheel in _currentWheels)
            {
                Destroy(transformAndWheel.Key.gameObject);
            }

            _currentWheels.Clear();
        }

        private void Update() 
        {
            if (_paused)
                return;

            foreach (var keyValuePair in _currentWheels)
            {
                keyValuePair.Key.position = keyValuePair.Value.WorldPosition;
                keyValuePair.Key.rotation = keyValuePair.Value.WorldRotation;
            }
        }
    }
}
