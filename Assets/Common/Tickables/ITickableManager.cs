namespace Common.Tickables
{
    public interface ITickableManager
    {
        void Register(ITickable tickable);
        void Remove(ITickable tickable);
    }
}