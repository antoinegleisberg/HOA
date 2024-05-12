using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
    public class ScriptableItem : ScriptableObject
    {
        [field:SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [TextArea]
        [SerializeField] private string _description;
        public string Description => _description;
        
        [field: SerializeField] public int StackSize { get; private set; }

        [field: SerializeField] public int HungerReplenish;
        [field: SerializeField] public int ThirstReplenish;

        public int ItemSize => ScriptableItemsDB.ItemStackSizesLCM() / StackSize;
    }
}
