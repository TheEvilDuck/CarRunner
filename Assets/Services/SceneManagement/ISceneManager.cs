using System;

namespace Services.SceneManagement
{
    public interface ISceneManager
    {
        public event Action beforeSceneLoadingStarted;
        public void LoadScene(string sceneId);
    }
}
