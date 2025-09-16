using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SimpleUnitySceneManager : ISceneManager
    {
        public event Action<string> sceneLoaded;
        public event Action<string> sceneLoadingRequested;

        public IEnumerator LoadScene(string sceneId)
        {
            sceneLoadingRequested?.Invoke(sceneId);

            if (!string.Equals(sceneId, SceneManager.GetActiveScene().name) && !string.Equals(sceneId, SceneIDs.BOOTSTRAP))
                yield return SceneManager.LoadSceneAsync(SceneIDs.BOOTSTRAP);
            
            yield return SceneManager.LoadSceneAsync(sceneId);

            sceneLoaded?.Invoke(sceneId);
        }
    }
}
