namespace Services.Localization
{
    public interface ILocalizable
    {
        public string TextId {get;}
        public void UpdateText(string text);
    }
}
