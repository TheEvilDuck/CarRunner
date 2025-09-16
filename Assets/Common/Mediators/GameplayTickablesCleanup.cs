using Common.Tickables;
using EntryPoint;
using GamePlay.Infrastructure;
using Infrastructure.DI;

namespace Common.Mediators
{
    public class GameplayTickablesCleanup: OnSceneChangedMediatorBase
    {
        private readonly TickableManager _gameplayTickables;
        private readonly TickableManager _globalTickables;
        
        public GameplayTickablesCleanup(IDIContainer container) : base(container)
        {
            _gameplayTickables = container.Get<TickableManager>(GameplayTags.TICKABLES);
            _globalTickables = container.Get<TickableManager>(EntryPointTags.PROJECT_TICKABLES_TAG);
        }

        protected override void OnSceneLoadingRequested(string sceneName)
            => _globalTickables.Remove(_gameplayTickables);
    }
}