using System;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SimpleUnitySceneManager : ISceneManager
    {
        public event Action beforeSceneLoadingStarted;

        public void LoadScene(string sceneId)
        {
            beforeSceneLoadingStarted?.Invoke();

            SceneManager.LoadScene(sceneId);
        }
    }
}
