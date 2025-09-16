using Infrastructure.DI;

namespace Infrastructure.Bootstraps
{
    public interface IDIRegistrator
    {
        public void MakeRegistrationsInto(DIContainer container);
    }
}