using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UIAnimatorSequence: MonoBehaviour
    {
        [SerializeField] private List<SequenseData> _animations;
        private int _currentAnimation = 0;
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
            StartCoroutine(PlayAnimation());
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
                if (_animations[_currentAnimation].animator.enabled)
                    StartCoroutine(PlayAnimation());
                else
                    PlayNext();
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
