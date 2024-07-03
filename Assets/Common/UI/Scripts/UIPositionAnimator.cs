using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UIPositionAnimator : UIAnimator
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private Vector2 _goal;
        private Vector2 _startPosition;

        private void Awake() 
        {
            _startPosition = _target.localPosition;

            if (_inverse)
            {
                EvaluateAnimation(1f);
            }
        }
        protected override void EvaluateAnimation(float strength)
        {
            _target.localPosition =  Vector2.Lerp(_startPosition, _startPosition + _goal, strength);
        }
    }

}