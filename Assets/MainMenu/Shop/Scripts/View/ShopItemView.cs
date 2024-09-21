using MainMenu.Shop.Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.Shop.View
{
    public abstract class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public UnityEvent Clicked => _button.onClick;

        public abstract void Init(ShopItem shopItem);
        public virtual void SetCurrencyImage(Sprite sprite) {}
        public virtual void SetItemImage(Sprite sprite) {}
    }
}
