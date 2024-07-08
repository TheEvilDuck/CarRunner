using System;

namespace DI
{
    internal abstract class ObjectData
    {
        private readonly DIContainer _dIContainer;

        public ObjectData(DIContainer dIContainer)
        {
            _dIContainer = dIContainer;
        }
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

        public ObjectData(DIContainer dIContainer,Func<T> createMethod): base(dIContainer)
        {
            _createMethod = createMethod;
        }

        public ObjectData(DIContainer dIContainer, T value): base(dIContainer)
        {
            _value = value;
        }

        public T Get()
        {
            if (_value == null)
                _value = _createMethod.Invoke();

            return _value;
        }
    }
}