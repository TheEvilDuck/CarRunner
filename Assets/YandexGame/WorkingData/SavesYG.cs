using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;


        // Ваши сохранения
        public List<string> AvailableLevels = new List<string>();
        public List<string> PassedLevels = new List<string>();
        public string SelectedLevel;
        public int Coins = 1000;
        public int MaxFallTries = 1;
        public bool IsTutorialComplete = false;
        public string savedLanguage;
        public string WatchShopAdLastTime;
        public SavesYG()
        {

        }
    }
}
