using System;
using TMPro;
using UnityEngine;

namespace Services.Localization
{
    public class LocalizableTextMeshPro : MonoBehaviour, ILocalizable
    {
        [SerializeField] private TextMeshProUGUI _target;
        [SerializeField] private string _textId;
        public string TextId => _textId;

        public event Action<ILocalizable> updateRequested;

        private void Start() 
        {
            LocalizationRegistrator.Instance.RegisterLocalizable(this);
            updateRequested?.Invoke(this);
        }

        public void UpdateText(string text)
        {
            _target.text = text;
        }
    }
}
