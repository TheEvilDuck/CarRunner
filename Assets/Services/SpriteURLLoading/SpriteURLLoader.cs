using System.Collections;
using Common.Reactive;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.SpriteURLLoading
{
    public class SpriteURLLoader
    {
        public bool debug;
        private Observable<Sprite> _loadedSprite = new Observable<Sprite>();
        public IReadonlyObservable<Sprite> LoadedSprite => _loadedSprite;

        public IEnumerator Load(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    if (debug)
                        Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    DownloadHandlerTexture handlerTexture = webRequest.downloadHandler as DownloadHandlerTexture;

                    if (handlerTexture.isDone)
                    {
                        Rect rect = new Rect(0, 0, handlerTexture.texture.width, handlerTexture.texture.height);
                        _loadedSprite.Value = Sprite.Create(handlerTexture.texture, rect, new Vector2(0.5f, 0.5f));
                    }
                }
            }
        }
    }
}
