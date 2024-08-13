using System;
using System.Collections.Generic;

namespace DI
{
    public class DIContainer
    {
        private readonly DIContainer _parent;
        private readonly Dictionary<(string, Type), ObjectData> _objects;
        private readonly HashSet<(string, Type)> _cyclicCash;

        public DIContainer(DIContainer parent = null)
        {
            _objects = new Dictionary<(string, Type), ObjectData>();
            _cyclicCash = new HashSet<(string, Type)>();
            _parent = parent;
        }

        public DIContainerBulder<T> Register<T>(Func<T> createFunc, string tag = "")
        {
            var tupple = (tag, typeof(T));

            if (_objects.ContainsKey(tupple))
                throw new ArgumentException($"There is already registered object with type of {typeof(T)} and tag {tag}");

            var objectData = new ObjectData<T>(createFunc);
            _objects.Add(tupple, objectData);
            return new DIContainerBulder<T>(this);
        }

        public DIContainerBulder<T> Register<T>(T value, string tag = "")
        {
            var tupple = (tag, typeof(T));

            if (_objects.ContainsKey(tupple))
                throw new ArgumentException($"There is already registered object with type of {typeof(T)} and tag {tag}");

            var objectData = new ObjectData<T>(value);
            _objects.Add(tupple, objectData);
            return new DIContainerBulder<T>(this);
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
                    return _parent.Get<T>();
            }
            finally
            {
                _cyclicCash.Remove(tupple);
            }

            throw new Exception($"DI container doesn't know how to create {tupple} and tag {tag}, do you forget to register entity?");
        }

        public class DIContainerBulder<T>
        {
            private readonly DIContainer _dIContainer;

            public DIContainerBulder(DIContainer dIContainer)
            {
                _dIContainer = dIContainer;
            }
            public void NonLazy()
            {
                _dIContainer.Get<T>();
            }
        }
    }
}
