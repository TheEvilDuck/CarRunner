using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UIRotationAnimator : UIAnimator
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private float _goalZRotation;
        private Quaternion _startRotation;

        private void Awake() 
        {
            _startRotation = _target.rotation;

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
            _target.rotation =  Quaternion.Euler(0, 0, _startRotation.eulerAngles.z + _goalZRotation * strength);
        }
    }

}