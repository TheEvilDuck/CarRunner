using Common.UI.UIAnimations;
using TMPro;
using UnityEngine;

namespace Gameplay.TimerGates
{
    public class TimerGateView : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI _text;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private GameObject _midPart;
        [SerializeField] private Color32 _goodColor;
        [SerializeField] private Color32 _badColor;
        [SerializeField] private UIAnimatorSequence _textAnimator;
        private TimerGate _timerGate;

        public void Init(TimerGate timerGate)
        {
            _timerGate = timerGate;
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            if (_timerGate.Time>=0)
            {
                _text.text = $"+{_timerGate.Time}";
                _renderer.material.SetColor("_TintColor", _goodColor);
            }
            else
            {
                _text.text = _timerGate.Time.ToString();
                _renderer.material.SetColor("_TintColor", _badColor);
            }

            _timerGate.passed += OnPassed;
        }

        private void OnDestroy() 
        {
            _timerGate.passed -= OnPassed;
        }

        private void OnPassed(float time)
        {
            _midPart.SetActive(false);
            _textAnimator.StartSequence();
        }
    }
}
