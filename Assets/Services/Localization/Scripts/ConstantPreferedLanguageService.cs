namespace Services.Localization.Scripts
{
    public class ConstantPreferedLanguageService: IPreferedLanguageService
    {
        private const string DEFAULT_LANGUAGE = "ru";
        public string GetPreferedLanguage() => DEFAULT_LANGUAGE;
    }
}