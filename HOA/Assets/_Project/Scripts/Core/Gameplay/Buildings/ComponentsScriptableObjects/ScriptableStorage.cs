using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName = "NewStorage", menuName = "ScriptableObjects/Buildings/Storage")]
    public class ScriptableStorage : ScriptableObject
    {
        [Tooltip("Used only for production and resource gathering sites")]
        public List<Pair<ScriptableItem, int>> ItemCapacities;

        [Tooltip("Used only for main storages")]
        public int MaxCapacity;
    }
}
