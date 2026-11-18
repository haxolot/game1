using UnityEngine;

namespace Haxolot.Data
{
    /// <summary>
    /// Modèle de données ScriptableObject pour les objets, armes, reliques et lore.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "Haxolot/Data/Item")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Identité")]
        public string id;
        public string itemName;
        public Sprite icon;

        [Header("Descriptions & Lore")]
        [TextArea(2, 4)]
        [Tooltip("Description courte des effets gameplay")]
        public string gameplayDescription;

        [TextArea(4, 10)]
        [Tooltip("Description narrative cryptique façon Dark Souls")]
        public string loreDescription;
    }
}
