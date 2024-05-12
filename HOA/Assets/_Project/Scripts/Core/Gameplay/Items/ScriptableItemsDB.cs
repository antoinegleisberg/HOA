using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public static class ScriptableItemsDB
    {
        private static readonly string _path = "Items";
        
        private static Dictionary<string, ScriptableItem> __items;

        private static int _itemStackSizesLCM = -1;

        private static Dictionary<string, ScriptableItem> _items {
            get
            {
                if (__items == null)
                {
                    Init();
                }
                return __items;
            }
        }

        public static ScriptableItem GetItemByName(string name)
        {
            if (!_items.ContainsKey(name))
            {
                Debug.LogError("ItemsDB does not contain an item with the name " + name);
                return null;
            }

            return _items[name];
        }

        public static int ItemStackSizesLCM()
        {
            if (_itemStackSizesLCM == -1)
            {
                _itemStackSizesLCM = ComputeItemStackSizesLCM();
            }

            return _itemStackSizesLCM;
        }
        
        public static void Reload()
        {
            Init();
        }

        private static void Init()
        {
            __items = new Dictionary<string, ScriptableItem>();

            ScriptableItem[] items = Resources.LoadAll<ScriptableItem>(_path);

            foreach (ScriptableItem item in items)
            {
                _items.Add(item.Name, item);
            }
        }
        
        public static int ComputeItemStackSizesLCM()
        {
            HashSet<int> stackSizes = new HashSet<int>();
            foreach (ScriptableItem item in _items.Values)
            {
                stackSizes.Add(item.StackSize);
            }
            
            return Math.Math.LCM(stackSizes.ToList());
        }
    }
}
