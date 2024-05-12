using antoinegleisberg.Types;
using System.Collections.Generic;
using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    public class ResourceSite : MonoBehaviour
    {
        [field: SerializeField] public ScriptableResourceSite ScriptableResourceSite { get; private set; }
        
        private int _remainingHarvests;

        public bool IsDepleted => _remainingHarvests <= 0;

        private void Awake()
        {
            _remainingHarvests = ScriptableResourceSite.MaxHarvests;
        }

        public Dictionary<ScriptableItem, int> Harvest()
        {
            if (IsDepleted)
            {
                return null;
            }

            _remainingHarvests--;
            return ScriptableResourceSite.AvailableItemsPerHarvest.ToDictionary();
        }
    }
}
