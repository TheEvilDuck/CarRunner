using System.Collections;
using Common.CoroutinePerformer;
using UnityEngine;

namespace Common.Tickables
{
    public class TickableManagerFactory
    {
        private readonly ICoroutinePerformer _coroutinePerformer;

        public TickableManagerFactory(ICoroutinePerformer coroutinePerformer)
        {
            _coroutinePerformer = coroutinePerformer;
        }

        public ITickableManager CreateWithCoroutinePerformer()
        {
            TickableManager tickableManager = new TickableManager();

            IEnumerator TickableRoutine()
            {
                tickableManager.Tick(Time.deltaTime);
                yield return null;
            }

            _coroutinePerformer.StartCoroutine(TickableRoutine());
            return tickableManager;
        }
    }
}