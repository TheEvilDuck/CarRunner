using YG;

namespace Common
{
    public class YandexGameLocalization
    {
        public string RequiredLanguage() => YandexGame.EnvironmentData.language;

        public void SwitchLanguage(string language) => YandexGame.SwitchLanguage(language);

        public void SwitchToDefaultLanguage() => YandexGame.SwitchLanguage("en");

        public void SetupYGLocalization()
        {
            switch (RequiredLanguage())
            {
                case "ru":
                    SwitchLanguage("ru");
                    break;
                case "en":
                    SwitchLanguage("en");
                    break;
                default:
                    SwitchToDefaultLanguage();
                    break;
            }
        }
    }
}