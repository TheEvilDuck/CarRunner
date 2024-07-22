using System;
using System.Collections;
using DI;
using UnityEngine;

namespace EntryPoint
{
    public abstract class MonoBehaviourBootstrap : MonoBehaviour
    {
        private DIContainer _projectContext;
        protected DIContainer _sceneContext;
        protected bool _inited;
        protected event Action _delayedStart;

        public void Init(DIContainer projectContext)
        {
            Debug.Log("A");
            _projectContext = projectContext;
            _sceneContext =  new DIContainer(_projectContext);
            Setup();
            _inited = true;
        }

        private IEnumerator Start() 
        {
            while (!_inited)
            {
                yield return new WaitForSeconds(0.1f);
            }

            _delayedStart?.Invoke();
        }

        protected virtual void Setup() {}
    }

}