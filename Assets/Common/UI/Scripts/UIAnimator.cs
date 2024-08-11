using System;
using System.Collections;
using UnityEngine;

namespace Common.UI.UIAnimations
{
    public abstract class UIAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField, Min(0)] private float _animationTime;
        [SerializeField] protected bool _inverse;
        [SerializeField] private bool _playOnStart;

        public event Action animationEnd;

        private Coroutine _currentAnimation;

        private void Start() 
        {
            if (_playOnStart)
                StartAnimation();
        }

        private void OnEnable() 
        {
            SetupAnimation();
        }

        public void StartAnimation()
        {
            StopAnimation();
            SetupAnimation();
            _currentAnimation = StartCoroutine(Animation());
        }

        public void StopAnimation()
        {
            if (_currentAnimation == null)
                return;

            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
            animationEnd?.Invoke();
        }

        private IEnumerator Animation()
        {
            float time = 0;

            while (time < _animationTime)
            {
                time += Time.deltaTime;
                float timePercent = time / _animationTime;
                float strength = _animationCurve.Evaluate(timePercent);

                if (_inverse)
                    strength = 1f - strength;

                EvaluateAnimation(strength);
                yield return null;
            }

            StopAnimation();
        }

        protected abstract void EvaluateAnimation(float strength);
        protected virtual void SetupAnimation() 
        {
            if (_inverse)
            {
                EvaluateAnimation(1f);
            }
        }
    }
}
