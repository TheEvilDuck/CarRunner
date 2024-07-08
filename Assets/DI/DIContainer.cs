using System;
using System.Collections.Generic;

namespace DI
{
    public class DIContainer
    {
        private readonly DIContainer _parent;
        private readonly Dictionary<Type, ObjectData> _objects;
        private readonly HashSet<Type> _cyclicCash;

        public DIContainer(DIContainer parent = null)
        {
            _objects = new Dictionary<Type, ObjectData>();
            _cyclicCash = new HashSet<Type>();
            _parent = parent;
        }

        public void Register<T>(Func<T> createFunc)
        {
            if (_objects.ContainsKey(typeof(T)))
                throw new ArgumentException($"There is already registered object with type of {typeof(T)}");

            var objectData = new ObjectData<T>(this, createFunc);
            _objects.Add(typeof(T), objectData);
        }

        public void Register<T>(T value)
        {
            if (_objects.ContainsKey(typeof(T)))
                throw new ArgumentException($"There is already registered object with type of {typeof(T)}");

            var objectData = new ObjectData<T>(this, value);
            _objects.Add(typeof(T), objectData);
        }

        public T Get<T>()
        {
            if (_cyclicCash.Contains(typeof(T)))
                throw new Exception($"Cyclic dependency accured with type of {typeof(T)}");

            _cyclicCash.Add(typeof(T));

            try
            {
                if (_objects.TryGetValue(typeof(T), out var objectData))
                    return objectData.Get<T>();

                if (_parent != null)
                    return _parent.Get<T>();
            }
            finally
            {
                _cyclicCash.Remove(typeof(T));
            }

            throw new Exception($"DI container doesn't know how to create {typeof(T)}, do you forget to register entity?");
        }
    }
}
