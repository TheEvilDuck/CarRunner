using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SimpleUnitySceneManager : ISceneManager
    {
        public event Action beforeSceneLoadingStarted;

        public async Awaitable LoadScene(string sceneId)
        {
            beforeSceneLoadingStarted?.Invoke();

            if (!string.Equals(sceneId, SceneManager.GetActiveScene().name) && !string.Equals(sceneId, SceneIDs.BOOTSTRAP))
                await SceneManager.LoadSceneAsync(SceneIDs.BOOTSTRAP);
            
            await SceneManager.LoadSceneAsync(sceneId);
        }
    }
}
