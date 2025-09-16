using System;
using System.Collections;
using UnityEngine;

namespace Services.SceneManagement
{
    public interface ISceneManager
    {
        public event Action<string> sceneLoaded;
        public event Action<string> sceneLoadingRequested;
        public IEnumerator LoadScene(string sceneId);
    }
}
