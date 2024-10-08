using System;
using UnityEngine;

namespace Services.SceneManagement
{
    public interface ISceneManager
    {
        public event Action beforeSceneLoadingStarted;
        public Awaitable LoadScene(string sceneId);
    }
}
