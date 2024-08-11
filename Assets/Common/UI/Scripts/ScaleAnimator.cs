using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class ScaleAnimator : UIAnimator
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _targetScale;
        [SerializeField] private bool _isLocal;
        private Vector3 _startScale;
        protected override void SetupAnimation()
        {
            if (_isLocal)
                _startScale = _target.localScale;
            else
                _startScale = _target.lossyScale;

            base.SetupAnimation();
        }
        protected override void EvaluateAnimation(float strength)
        {
            Vector3 result = Vector3.Lerp(_startScale, _targetScale + _startScale, strength);
            _target.localScale = result;
        }
    }
}
