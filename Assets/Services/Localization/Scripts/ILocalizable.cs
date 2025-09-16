using System;

namespace Services.Localization.Scripts
{
    public interface ILocalizable
    {
        public string TextId {get;}
        public event Action<ILocalizable> updateRequested;
        public void UpdateText(string text);
    }
}
