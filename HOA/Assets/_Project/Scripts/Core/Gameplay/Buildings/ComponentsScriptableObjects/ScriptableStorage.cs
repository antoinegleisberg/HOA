using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "NewStorage", menuName = "ScriptableObjects/Buildings/Storage")]
    public class ScriptableStorage : ScriptableObject
    {
        [Tooltip("Used only for production and resource gathering sites")]
        [SerializeField] private List<Pair<ScriptableItem, int>> _itemCapacities;

        [Tooltip("Used only for main storages")]
        [SerializeField] public int _maxCapacity;

        public Dictionary<ScriptableItem, int> ItemCapacities => _itemCapacities.ToDictionary();
        public int MaxCapacity => _maxCapacity;

        private void OnValidate()
        {
            if (_maxCapacity < 0)
            {
                Debug.LogWarning("Max capacity cannot be negative. Setting it to 0.");
                _maxCapacity = 0;
            }
        }
    }
}
