using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UITransparancyAnimator : UIAnimator
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Range(0, 1f)] private float _targetAlpha;
        private float _startAlpha;

        private void Awake() 
        {
            _startAlpha = _canvasGroup.alpha;

            if (_inverse)
            {
                EvaluateAnimation(1f);
            }
        }

        protected override void SetupAnimation()
        {
            if (_inverse)
            {
                EvaluateAnimation(1f);
            }
        }
        protected override void EvaluateAnimation(float strength)
        {
            _canvasGroup.alpha = Mathf.Lerp(_startAlpha, _targetAlpha, strength);
        }
    }
}
