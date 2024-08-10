using Common;
using Common.MenuParent;
using Common.UI.UIAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundSettingsView : MonoBehaviour, IMenuParent
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

    public void Init(ISoundSettings soundSettings)
    {
        _toggleMute.isOn = soundSettings.Muted;
        _masterVolume.value = soundSettings.MasterVolume;
        _backgroundMusicVolume.value = soundSettings.BackgroundMusicVolume;
        _SFXSoundsVolume.value = soundSettings.SFXSoundVolume;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _uIAnimatorSequence.StartSequence();
    }

    public void Hide() => gameObject.SetActive(false);
}
