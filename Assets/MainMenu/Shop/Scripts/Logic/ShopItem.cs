using System;
using DI;
using UnityEngine;

namespace MainMenu.Shop.Logic
{
    public abstract class ShopItem : ScriptableObject
    {
        public event Action claimed;

        public abstract bool TryClaim(DIContainer sceneContext);
        public virtual void Init(DIContainer sceneContext) {}

        protected void Claim() => claimed?.Invoke();
    }
}
