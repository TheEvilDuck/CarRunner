using Common.MenuParent;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UI
{
    public class SettingsMenu : MonoBehaviour, IMenuParent
    {
        private MenuParentsManager _menuParentsManager;
        [SerializeField] private SettingsMenuButtons _settingsMenuButtons;
        [field: SerializeField] public SoundSettingsView SoundSettingsView { get; private set; }
        [field: SerializeField] public CameraSettingsView CameraSettingsView { get; private set; }

        public UnityEvent BackPressed => _settingsMenuButtons.BackPressed;

        public void Init()
        {
            _menuParentsManager = new MenuParentsManager();
            _menuParentsManager.Add(SoundSettingsView);
            _menuParentsManager.Add(CameraSettingsView);
            _menuParentsManager.Add(_settingsMenuButtons);
        }

        private void OnEnable()
        {
            _settingsMenuButtons.SoundSettingsPressed.AddListener(OnSoundSettingsButtonPressed);
            _settingsMenuButtons.CameraSettingsPressed.AddListener(OnCameraSettingsButtonPressed);
            SoundSettingsView.BackPressed.AddListener(ShowMainButtons);
            CameraSettingsView.BackPressed.AddListener(ShowMainButtons);
        }

        private void OnDisable()
        {
            _settingsMenuButtons.SoundSettingsPressed.RemoveListener(OnSoundSettingsButtonPressed);
            _settingsMenuButtons.CameraSettingsPressed.RemoveListener(OnCameraSettingsButtonPressed);
            SoundSettingsView.BackPressed.RemoveListener(ShowMainButtons);
            CameraSettingsView.BackPressed.RemoveListener(ShowMainButtons);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _menuParentsManager.Show(_settingsMenuButtons);
        }

        public void Hide() => gameObject.SetActive(false);

        private void OnSoundSettingsButtonPressed() => _menuParentsManager.Show(SoundSettingsView);

        private void OnCameraSettingsButtonPressed() => _menuParentsManager.Show(CameraSettingsView);

        private void ShowMainButtons() => _menuParentsManager.Show(_settingsMenuButtons);
    }
}