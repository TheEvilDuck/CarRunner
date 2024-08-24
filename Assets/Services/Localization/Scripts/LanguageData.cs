using UnityEngine;

namespace Services.Localization
{
    [CreateAssetMenu(menuName = "Localization/New languageData", fileName = "LanguageData")]
    public class LanguageData : ScriptableObject
    {
        [field: SerializeField] public string LanguageId {get; private set;}
        [field: SerializeField] public string LanguageNativeName {get; private set;}
        [field: SerializeField] public Sprite LangiageIcon {get; private set;}
    }
}
