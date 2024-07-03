using Common;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _backgroundMusicVolume;
        [SerializeField] private Slider _SFXSoundsVolume;
        [SerializeField] private Toggle _toggleMute;
        [SerializeField] private UIAnimatorSequence _uIAnimatorSequence;

        public UnityEvent BackPressed => _backButton.onClick;
        public UnityEvent<float> MasterVolumeChanged => _masterVolume.onValueChanged;
        public UnityEvent<float> BackgroundMusicVolumeChanged => _backgroundMusicVolume.onValueChanged;
        public UnityEvent<float> SFXSoundVolumeChanged => _SFXSoundsVolume.onValueChanged;
        public UnityEvent<bool> MuteChanged => _toggleMute.onValueChanged;

        private void OnEnable() 
        {
            _uIAnimatorSequence.StartSequence();
        }

        public void Init(GameSettings gameSettings)
        {
            _toggleMute.isOn = gameSettings.Muted;
            _masterVolume.value = gameSettings.MasterVolume;
            _backgroundMusicVolume.value = gameSettings.BackgroundMusicVolume;
            _SFXSoundsVolume.value = gameSettings.SFXSoundVolume;
        }
    }
}