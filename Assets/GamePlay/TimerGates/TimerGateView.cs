using TMPro;
using UnityEngine;

namespace Gameplay.TimerGates
{
    public class TimerGateView : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI _text;

        public void Init(float time)
        {
            if (time>=0)
                _text.text = $"+{time}";
            else
                _text.text = time.ToString();
        }
    }
}
