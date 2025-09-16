using System.Collections.Generic;

namespace Common.Tickables
{
    public class TickableManager : ITickableManager
    {
        private readonly List<ITickable> _tickables = new List<ITickable>();
        
        public void Register(ITickable tickable) => _tickables.Add(tickable);
        public void Remove(ITickable tickable) => _tickables.Remove(tickable);

        public void Tick(float deltaTime)
        {
            for (int i = _tickables.Count - 1; i >= 0; i--)
                _tickables[i]?.Tick(deltaTime);
        }
    }
}