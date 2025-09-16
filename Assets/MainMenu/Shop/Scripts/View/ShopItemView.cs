using Infrastructure.DI;
using MainMenu.Shop.Scripts.Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.Shop.Scripts.View
{
    public abstract class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public UnityEvent Clicked => _button.onClick;

        public abstract void Init(ShopItem shopItem, IDIContainer container);
    }
}
