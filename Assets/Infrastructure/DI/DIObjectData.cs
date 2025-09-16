using System;

namespace Infrastructure.DI
{
    internal abstract class ObjectData
    {
        public abstract bool NotCreatedYet { get; }
        
        public T Get<T>()
        {
            var casted = (ObjectData<T>)this;
            return casted.Get();
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

        public T Get()
        {
            if (NotCreatedYet)
                _value = _createMethod.Invoke();

            return _value;
        }
    }
}