namespace Infrastructure.DI
{
    public interface IDIContainer
    {
        public T Get<T>(string tag = "");
    }
}