using System;
using System.Collections;
using Common.CoroutinePerformer;
using Common.Reactive;
using Services.Localization.Scripts;

namespace Services.PlayerData.Core.Language
{
    public class LanguageService: DataService<ILanguageData>, ILanguageService
    {
        public override event Action DataChanged;
        
        private readonly IPreferedLanguageService _preferedLanguageService;

        private readonly Observable<string> _language;
        
        public IReadonlyObservable<string> Language => _language;
        
        public LanguageService(
            IDataProvider<ILanguageData> dataProvider, 
            ICoroutinePerformer coroutinePerformer, 
            IPreferedLanguageService preferedLanguageService) : base(dataProvider, coroutinePerformer)
        {
            _preferedLanguageService = preferedLanguageService;
            _language = new Observable<string>(_preferedLanguageService.GetPreferedLanguage());
        }
        
        protected override IEnumerator InitializeInternal(ILanguageData data)
        {
            _language.Value = data.Language;
            yield break;
        }
        
        public void SetLanguage(string language)
        {
            if (string.Equals(language, _language.Value))
                return;
            
            _language.Value = language;
            DataChanged?.Invoke();
        }

        protected override IEnumerator LoadDefaultData(DataLoadCallback<ILanguageData> callback)
        {
            string preferedLanguage = _preferedLanguageService.GetPreferedLanguage();
            callback?.Invoke(true, new LanguageData(preferedLanguage));
            yield break;
        }

        protected override ILanguageData GetData() => new LanguageData(_language.Value);

        private class LanguageData : ILanguageData
        {
            public string Language { get; }
            
            public LanguageData(string language)
            {
                Language = language;
            }
        }
    }
}