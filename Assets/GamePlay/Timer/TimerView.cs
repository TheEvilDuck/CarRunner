using TMPro;
using UnityEngine;
namespace Gameplay.UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        public void ChangeValue(float time)
        {
            float minutes = time/60f;
            float seconds = time%60f;
            _timerText.text = $"{Mathf.Floor(minutes).ToString("00")}:{seconds.ToString("00.00")}";
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
