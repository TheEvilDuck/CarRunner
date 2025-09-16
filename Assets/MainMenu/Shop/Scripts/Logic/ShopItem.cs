using System;
using System.Collections;
using Infrastructure.DI;
using UnityEngine;

namespace MainMenu.Shop.Scripts.Logic
{
    public abstract class ShopItem : ScriptableObject
    {
        public event Action claimed;

        public IEnumerator TryClaim(IDIContainer container, Action<bool> callback)
        {
            if (CanBeClaimed(container) == false)
            {
                callback?.Invoke(false);
                yield break;
            }

            void OnPurchaseProceed(bool success)
            {
                if (success)
                    claimed?.Invoke(); 
                
                callback?.Invoke(success);
            }

            yield return Claim(container, OnPurchaseProceed);
        }
        public abstract bool CanBeClaimed(IDIContainer container);
        protected abstract IEnumerator Claim(IDIContainer container, Action<bool> callback);
    }
}
