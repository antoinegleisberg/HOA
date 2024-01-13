using UnityEngine;


namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
    public class ScriptableItem : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        [TextArea]
        public string Description;
        public int StackSize;

        public int ItemSize
        {
            get
            {
                return ScriptableItemsDB.ItemStackSizesLCM() / StackSize;
            }
        }
    }
}
