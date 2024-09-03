using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UIAnimatorSequence: MonoBehaviour
    {
        [SerializeField] private bool _loop;
        [SerializeField] private List<SequenseData> _animations;

        public event Action end;
        private int _currentAnimation = 0;
        private Coroutine _currentCoroutine;
        private void OnDisable() 
        {
            if (_currentAnimation < _animations.Count)
            {
                _animations[_currentAnimation].animator.animationEnd -= PlayNext;
                _animations[_currentAnimation].animator.StopAnimation();
            }
        }
        public void StartSequence()
        {
            _currentAnimation = 0;

            if (_currentCoroutine != null)
                StopSequence();

            _currentCoroutine = StartCoroutine(PlayAnimation());
        }

        public void StopSequence()
        {
            _animations[_currentAnimation].animator.animationEnd -= PlayNext;
            _animations[_currentAnimation].animator.StopAnimation();
            _currentAnimation = 0;

            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = null;
        }

        public void AddAnimation(UIAnimator uIAnimator, float delay, bool nextAnimationWait)
        {
            SequenseData sequenseData = new SequenseData
            {
                animator = uIAnimator,
                delay = delay,
                nextAnimationWait = nextAnimationWait
            };

            _animations.Add(sequenseData);
        }

        private IEnumerator PlayAnimation()
        {
            yield return new WaitForSeconds(_animations[_currentAnimation].delay);
            _animations[_currentAnimation].animator.StartAnimation();

            if (_animations[_currentAnimation].nextAnimationWait)
            {
                _animations[_currentAnimation].animator.animationEnd += PlayNext;
            }
            else
            {
                PlayNext();
            }
        }

        private void PlayNext()
        {
            _animations[_currentAnimation].animator.animationEnd -= PlayNext;

            _currentAnimation ++;

            if (_currentAnimation < _animations.Count)
            {
                if (_animations[_currentAnimation].animator.enabled && _animations[_currentAnimation].animator.isActiveAndEnabled)
                    StartCoroutine(PlayAnimation());
                else
                    PlayNext();
            }
            else if (_loop)
            {
                _currentAnimation = 0;
                StartCoroutine(PlayAnimation());
            }
            else
            {
                end?.Invoke();
            }
        }

        [Serializable]
        private struct SequenseData
        {
            public UIAnimator animator;
            public float delay;
            public bool nextAnimationWait;
        }
    }
}
