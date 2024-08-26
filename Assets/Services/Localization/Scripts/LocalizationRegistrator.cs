namespace Services.Localization
{
    public class LocalizationRegistrator
    {
        private static LocalizationRegistrator _instance;
        private Localizator _localizator;

        public static LocalizationRegistrator Instance
        {
            get
            {
                return _instance;
            }
        }

        public LocalizationRegistrator(Localizator localizator)
        {
            if (_instance != null)
                throw new System.Exception($"You are trying to make more than one {typeof(LocalizationRegistrator)}. That's now allowed because it's singletone");

            _instance = this;
            _localizator = localizator;
        }

        public void RegisterLocalizable(ILocalizable localizable, bool instantTranslate = true)
        {
            _localizator.RegisterLocalizable(localizable, instantTranslate);
        }
    }
}
