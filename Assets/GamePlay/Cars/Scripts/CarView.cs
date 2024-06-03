using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cars
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        Dictionary<Transform,IReadOnlyWheel> _currentWheels;
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

                _currentWheels.Add(wheelView.transform, wheel);
                
            }
        }

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
            foreach (var keyValuePair in _currentWheels)
            {
                keyValuePair.Key.position = keyValuePair.Value.WorldPosition;
                keyValuePair.Key.rotation = keyValuePair.Value.WorldRotation;
            }
        }
    }
}
