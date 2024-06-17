using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Slider _MasterVolume;
        [SerializeField] private Slider _BackgroundMusicVolume;
        [SerializeField] private Slider _SFXSoundsVolume;
        [SerializeField] private Toggle _toggleIsMusicOn;

        public UnityEvent BackPressed => _backButton.onClick;
        public UnityEvent<float> MasterVolumeChanged => _MasterVolume.onValueChanged;
        public UnityEvent<float> BackgroundMusicVolumeChanged => _BackgroundMusicVolume.onValueChanged;
        public UnityEvent<float> SFXSoundVolumeChanged => _SFXSoundsVolume.onValueChanged;
        public UnityEvent<bool> MusicOnChanged => _toggleIsMusicOn.onValueChanged;
    }
}