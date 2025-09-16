using System.Collections;
using UnityEngine;

namespace Common.CoroutinePerformer
{
    public interface ICoroutinePerformer
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
        public void StopCoroutine(Coroutine coroutine);
        public void StopAllCoroutines();
    }
}