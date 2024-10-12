using System;
using UnityEngine;

namespace Services.SceneManagement
{
    public interface ISceneManager
    {
        public event Action beforeSceneLoadingStarted;
        public event Action afterScemeLoadingEnd;
        public Awaitable LoadScene(string sceneId);
    }
}
