using System;
using System.Collections;
using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UIAnimatorSequence: MonoBehaviour
    {
        [SerializeField] private SequenseData[] _animations;
        private int _currentAnimation = 0;

        private void OnEnable() 
        {
            _currentAnimation = 0;
            StartSequence();
        }

        private void OnDisable() 
        {
            if (_currentAnimation < _animations.Length)
            {
                _animations[_currentAnimation].animator.animationEnd -= PlayNext;
                _animations[_currentAnimation].animator.StopAnimation();
            }
        }
        public void StartSequence()
        {
            StartCoroutine(PlayAnimation());
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

            if (_currentAnimation < _animations.Length)
                StartCoroutine(PlayAnimation());
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
