using YG;

namespace Common
{
    public class YandexGameGameplay : IPausable
    {
        public void Pause()
        {
            YandexGame.GameplayStop();
        }

        public void Resume()
        {
            YandexGame.GameplayStart();
        }
    }
}
