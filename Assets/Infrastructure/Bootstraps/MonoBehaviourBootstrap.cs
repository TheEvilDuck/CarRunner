using System.Collections;
using Infrastructure.DI;
using UnityEngine;

namespace Infrastructure.Bootstraps
{
    public abstract class MonoBehaviourBootstrap: MonoBehaviour, IDIRegistrator, IBootstrapInitializer
    {
        public abstract void MakeRegistrationsInto(DIContainer container);

        public IEnumerator Initialize(IDIContainer container)
        {
            IBootstrapInitializer innerInitializer = GetInnerInitializer(container);
            yield return innerInitializer.Initialize(container);
        }

        protected abstract IBootstrapInitializer GetInnerInitializer(IDIContainer container);
    }

}