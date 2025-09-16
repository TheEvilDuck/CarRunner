using System.Collections;
using Infrastructure.DI;

namespace Infrastructure.Bootstraps
{
    public interface IBootstrapInitializer
    {
        public IEnumerator Initialize(IDIContainer container);
    }
}