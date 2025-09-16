using System.Collections;
using Infrastructure.DI;

namespace Infrastructure.Bootstraps
{
    public class Bootstrap
    {
        private readonly IDIRegistrator _registrator;
        private readonly IBootstrapInitializer _initializer;
        private readonly DIContainer _container;

        public Bootstrap(
            IDIRegistrator registrator,
            IBootstrapInitializer initializer,
            IDIContainer parentContainer = null)
        {
            _registrator = registrator;
            _initializer = initializer;
            
            _container = new DIContainer(parentContainer);
        }

        public IEnumerator Run()
        {
            _registrator.MakeRegistrationsInto(_container);
            _container.Initialize();

            yield return _initializer.Initialize(_container);
        }
        
    }
}