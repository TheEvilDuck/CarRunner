using Common.MenuParent;
using Common.UI.UIAnimations;
using DI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu.Shop.View
{
    public class ShopView : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private UIAnimatorSequence _animations;
        [SerializeField] private Transform _shopItemViewContent;

        public UnityEvent BackPressed => _backButton.onClick;
        public void Init(ShopItemFactory shopItemFactory, DIContainer sceneContext)
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
