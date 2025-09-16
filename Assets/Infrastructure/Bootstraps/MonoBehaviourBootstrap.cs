using System;
using System.Collections;
using Infrastructure.DI;
using Services.SceneManagement;
using UnityEngine;

namespace Infrastructure.Bootstraps
{
    public abstract class MonoBehaviourBootstrap: MonoBehaviour, IDIRegistrator, IBootstrapInitializer
    {
        public abstract void MakeRegistrationsInto(DIContainer container);
        public abstract IEnumerator Initialize(IDIContainer container);
    }

}