using System;
using System.Collections.Generic;
using Common.Disposables;
using Common.Tickables;
using UnityEngine;

namespace Infrastructure.DI
{
    public class DIContainer: IDIContainer
    {
        private readonly IDIContainer _parent;
        private readonly Dictionary<(string, Type), ObjectData> _objects;
        private readonly HashSet<(string, Type)> _cyclicCash;
        private readonly List<Action> _onInitializedCallbacks;

        public DIContainer(IDIContainer parent = null)
        {
            _objects = new Dictionary<(string, Type), ObjectData>();
            _cyclicCash = new HashSet<(string, Type)>();
            _onInitializedCallbacks = new List<Action>();
            _parent = parent;
        }

        public void Initialize()
        {
            foreach(Action onInitializedCallback in _onInitializedCallbacks)
                onInitializedCallback?.Invoke();
        }

        public DIContainerBulder<T> Register<T>(Func<T> createFunc, string tag = "")
        {
            var tupple = (tag, typeof(T));

            if (_objects.ContainsKey(tupple))
                throw new ArgumentException($"There is already registered object with type of {typeof(T)} and tag {tag}");

            var objectData = new ObjectData<T>(createFunc);
            _objects.Add(tupple, objectData);
            return new DIContainerBulder<T>(objectData, _onInitializedCallbacks, this);
        }

        public DIContainerBulder<T> Register<T>(T value, string tag = "")
        {
            var tupple = (tag, typeof(T));

            if (_objects.ContainsKey(tupple))
                throw new ArgumentException($"There is already registered object with type of {typeof(T)} and tag {tag}");

            var objectData = new ObjectData<T>(value);
            _objects.Add(tupple, objectData);
            return new DIContainerBulder<T>(objectData, _onInitializedCallbacks, this);
        }

        public T Get<T>(string tag = "")
        {
            var tupple = (tag, typeof(T));

            if (_cyclicCash.Contains(tupple))
                throw new Exception($"Cyclic dependency accured with type of {tupple} and tag {tag}");

            _cyclicCash.Add(tupple);

            try
            {
                if (_objects.TryGetValue(tupple, out var objectData))
                    return objectData.Get<T>();

                if (_parent != null)
                    return _parent.Get<T>(tag);
            }
            finally
            {
                _cyclicCash.Remove(tupple);
            }

            throw new Exception($"DI container doesn't know how to create {tupple} and tag {tag}, do you forget to register entity?");
        }

        public class DIContainerBulder<T>
        {
            private readonly DIContainer _container;
            private readonly ObjectData _objectData;
            private readonly List<Action> _onInitializedCallbacks;

            internal DIContainerBulder(
                ObjectData objectData, 
                List<Action> onInitializedCallbacks, 
                DIContainer container)
            {
                _objectData = objectData;
                _onInitializedCallbacks = onInitializedCallbacks;
                _container = container;
            }

            public DIContainerBulder<T> AddToDisposables(string disposableTag = null)
            {
                _onInitializedCallbacks.Add(() =>
                {
                    DisposableDelegate disposableDelegate = new DisposableDelegate(() =>
                    {
                        if (_objectData.NotCreatedYet)
                            return;
                        
                        T value = _objectData.Get<T>();
                        
                        if (value is IDisposable disposable)
                            disposable?.Dispose();
                    });
                    
                    _container.Get<CompositeDisposable>(disposableTag).Add(disposableDelegate);
                });

                return this;
            }
            
            public DIContainerBulder<T> AddToTickables(string tickablesTag = null)
            {
                _onInitializedCallbacks.Add(() =>
                {
                    if (_objectData.NotCreatedYet)
                    {
                        void OnCreated()
                        {
                            Debug.Log("LAZY CREATED");
                            _objectData.Created -= OnCreated;
                            Register(_objectData.Get<T>());
                        }

                        _objectData.Created += OnCreated;
                    }
                    else
                    {
                        Register(_objectData.Get<T>());
                    }

                    void Register(T value)
                    {
                        if (value is not ITickable tickable)
                            return;
                    
                        _container.Get<TickableManager>(tickablesTag).Register(tickable);
                    }
                });

                return this;
            }

            public void NonLazy()
            {
                _onInitializedCallbacks.Add(() => _objectData.Get<T>());
            }
        }
    }
}
