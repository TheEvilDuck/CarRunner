using System;

namespace Common.Reactive
{
    public interface IReadonlyObservable<T>
    {
        public event Action<T> changed;
        public T Value {get;}
    }

}