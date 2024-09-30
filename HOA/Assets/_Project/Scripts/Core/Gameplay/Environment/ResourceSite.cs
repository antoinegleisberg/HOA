using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    public class ResourceSite : MonoBehaviour
    {
        [field: SerializeField] public ScriptableResourceSite ScriptableResourceSite { get; private set; }
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

        private int _remainingHarvestsAtCurrentStage;
        private int _currentStageIndex;

        public bool IsDepleted => _remainingHarvestsAtCurrentStage <= 0;

        private void Awake()
        {
            _currentStageIndex = 0;
            _remainingHarvestsAtCurrentStage = ScriptableResourceSite.HarvestStageInfos[_currentStageIndex].NumberOfHarvests;
        }

        public IReadOnlyDictionary<ScriptableItem, int> Harvest()
        {
            if (IsDepleted)
            {
                throw new Exception("Resource site is depleted");
            }

            _remainingHarvestsAtCurrentStage--;
            if (_remainingHarvestsAtCurrentStage <= 0)
            {
                UpdateHarvestInfo(_currentStageIndex + 1);
            }

            return ScriptableResourceSite.AvailableItemsPerHarvest;
        }

        private void UpdateHarvestInfo(int newStageIndex)
        {
            _currentStageIndex = newStageIndex;
            if (_currentStageIndex >= ScriptableResourceSite.HarvestStageInfos.Count)
            {
                _remainingHarvestsAtCurrentStage = 0;
                StartCoroutine(Replenish());
            }
            else
            {
                _remainingHarvestsAtCurrentStage = ScriptableResourceSite.HarvestStageInfos[_currentStageIndex].NumberOfHarvests;
                SpriteRenderer.sprite = ScriptableResourceSite.HarvestStageInfos[_currentStageIndex].Sprite;
            }
        }

        private IEnumerator Replenish()
        {
            if (!ScriptableResourceSite.Replenishes)
            {
                Destroy(gameObject);
                yield break;
            }

            foreach (GrowthStage growthState in ScriptableResourceSite.GrowthStages)
            {
                SpriteRenderer.sprite = growthState.Sprite;
                yield return new WaitForSeconds(growthState.GrowthTime);
            }
            UpdateHarvestInfo(0);
        }
    }
}
