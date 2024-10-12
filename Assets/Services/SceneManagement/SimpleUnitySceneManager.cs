using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SimpleUnitySceneManager : ISceneManager
    {
        public event Action beforeSceneLoadingStarted;
        public event Action afterScemeLoadingEnd;

        public async Awaitable LoadScene(string sceneId)
        {
            beforeSceneLoadingStarted?.Invoke();

            if (!string.Equals(sceneId, SceneManager.GetActiveScene().name) && !string.Equals(sceneId, SceneIDs.BOOTSTRAP))
                await SceneManager.LoadSceneAsync(SceneIDs.BOOTSTRAP);

            await Awaitable.WaitForSecondsAsync(0.5f);
            
            await SceneManager.LoadSceneAsync(sceneId);

            afterScemeLoadingEnd?.Invoke();
        }
    }
}
