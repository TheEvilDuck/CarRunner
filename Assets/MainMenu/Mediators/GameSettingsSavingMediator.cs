using Common.Mediators;
using Common.Settings;
using Infrastructure.DI;

namespace MainMenu.Mediators
{
    public class GameSettingsSavingMediator: OnSceneChangedMediatorBase
    {
        private readonly GameSettings _gameSettings;

        public GameSettingsSavingMediator(IDIContainer container) : base(container)
        {
            _gameSettings = container.Get<GameSettings>();
        }

        protected override void OnSceneLoadingRequested(string sceneName)
            => _gameSettings.SaveSettings();
    }
}