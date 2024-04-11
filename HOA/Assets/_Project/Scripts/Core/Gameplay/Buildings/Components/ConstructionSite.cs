using antoinegleisberg.Types;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace antoinegleisberg.HOA
{
    [RequireComponent(typeof(Building), typeof(Storage), typeof(Workplace))]
    public class ConstructionSite : MonoBehaviour
    {
        private Storage _storage => GetComponent<Storage>();

        public bool CanBuild()
        {
            return MissingItems().Count == 0;
        }

        public void FinishConstruction()
        {
            if (!CanBuild())
            {
                throw new Exception("Missing building materials !");
            }
            BuildingsBuilder.Instance.BuildBuilding(this);
        }

        public Dictionary<ScriptableItem, int> MissingItems()
        {
            Dictionary<ScriptableItem, int> requiredItems = GetComponent<Building>().ScriptableBuilding.BuildingMaterials.ToDictionary();

            Dictionary<ScriptableItem, int> missingItems = new Dictionary<ScriptableItem, int>();
            foreach (ScriptableItem item in requiredItems.Keys)
            {
                int missingAmount = requiredItems[item] - _storage.GetItemCount(item);
                missingItems.Add(item, missingAmount);
            }
            return missingItems;
        }
    }
}
