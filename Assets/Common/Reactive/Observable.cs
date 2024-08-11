using System;

namespace Common.Reactive
{
    public class Observable<T> : IReadonlyObservable<T>
    {
        private T _value;
        public event Action<T> changed;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                changed?.Invoke(_value);
            }
        }
    }
}
