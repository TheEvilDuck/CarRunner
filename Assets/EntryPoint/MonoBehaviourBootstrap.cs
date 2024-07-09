using DI;
using UnityEngine;

namespace EntryPoint
{
    public abstract class MonoBehaviourBootstrap : MonoBehaviour
    {
        private DIContainer _projectContext;
        protected DIContainer _sceneContext;
        protected bool _inited;

        public void Init(DIContainer projectContext)
        {
            _projectContext = projectContext;
            _sceneContext =  new DIContainer(_projectContext);
            Setup();
            _inited = true;
        }

        protected abstract void Setup();
    }

}