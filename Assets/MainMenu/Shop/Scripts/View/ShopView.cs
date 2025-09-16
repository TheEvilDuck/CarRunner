using Common.MenuParent;
using Common.UI.Scripts;
using Infrastructure.DI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.Shop.Scripts.View
{
    public class ShopView : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private UIAnimatorSequence _animations;
        [SerializeField] private Transform _shopItemViewContent;

        public UnityEvent BackPressed => _backButton.onClick;

        public void Init(ShopItemFactory shopItemFactory, IDIContainer sceneContext)
        {
            var items = shopItemFactory.GetSetUpView(_shopItemViewContent, sceneContext);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _animations.StartSequence();
        }
    }
}
