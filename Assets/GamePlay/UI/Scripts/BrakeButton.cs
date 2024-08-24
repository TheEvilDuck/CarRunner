using Services.PlayerInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class BrakeButton : MonoBehaviour, IBrakeButton, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsBraking {get; private set;}
        private RectTransform _rectTransform;

        private void Awake() 
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            IsBraking = false;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
        }

        public bool IsScreenPositionInside(Vector2 position)
        {
            if (!IsBraking)
                return false;

            return RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, position);
        }

        public void OnPointerDown(PointerEventData eventData) => IsBraking = true;

        public void OnPointerUp(PointerEventData eventData) => IsBraking = false;

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
            transform.localScale = Vector3.one;
        }
    }
}
