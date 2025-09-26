using Common.Reactive;

namespace Services.PlayerData.Core.Language
{
    public interface ILanguageService
    {
        public IReadonlyObservable<string> Language { get; }
        public void SetLanguage(string language);
    }
}