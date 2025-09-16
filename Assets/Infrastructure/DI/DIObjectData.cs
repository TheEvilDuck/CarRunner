using System;
using UnityEngine;

namespace Infrastructure.DI
{
    internal abstract class ObjectData
    {
        public event Action Created;
        public abstract bool NotCreatedYet { get; }
        
        public T Get<T>()
        {
            var casted = (ObjectData<T>)this;

            T instance = casted.Get(out bool created);

            if (created)
            {
                Created?.Invoke();
            }
            
            return instance;
        }
    }

    internal class ObjectData<T>: ObjectData
    {
        private readonly Func<T> _createMethod;
        private T _value;

        public override bool NotCreatedYet => _value == null || _value is UnityEngine.Object value && value == null;

        public ObjectData(Func<T> createMethod)
        {
            _createMethod = createMethod;
        }

        public ObjectData(T value)
        {
            _value = value;
        }

        public T Get(out bool created)
        {
            if (NotCreatedYet)
            {
                _value = _createMethod.Invoke();
                created = true;
            }
            else
            {
                created = false;
            }
            
            return _value;
        }
    }
}