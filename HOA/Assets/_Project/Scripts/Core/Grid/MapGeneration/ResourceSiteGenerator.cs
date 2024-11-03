using antoinegleisberg.Types;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public class ResourceSiteGenerator : MonoBehaviour
    {
        [SerializeField] private List<Pair<ScriptableResourceSite, float>> _resourceSiteGenerationDistribution;

        [SerializeField] private float _resourceSiteGenerationProbability = 0.1f;

        [SerializeField] private Transform _resourceSitesContainer;
        [SerializeField] private ResourceSite _resourceSitePrefab;

        private Dictionary<Vector3Int, ResourceSite> _resourceSites;
        public IReadOnlyDictionary<Vector3Int, ResourceSite> ResourceSites => _resourceSites;

        [SerializeField] private float _resourceSiteFreeRange = 5f;

        private void Awake()
        {
            _resourceSites = new Dictionary<Vector3Int, ResourceSite>();
        }

        public ResourceSite GetResourceSiteAt(Vector3Int position)
        {
            if (_resourceSites.ContainsKey(position))
            {
                return _resourceSites[position];
            }
            return null;
        }

        public void CheckForResourceSiteGeneration(Vector3Int position)
        {
            if (Random.value >= _resourceSiteGenerationProbability)
            {
                return;
            }
            if (!MapManager.Instance.GetTileAt(position).TileType.CanSpawnResourceSite)
            {
                return;
            }
            Vector3 worldPosition = GridManager.Instance.CellToWorldPosition(position) + new Vector3(0, 0.25f, 0);
            if (worldPosition.magnitude < _resourceSiteFreeRange)
            {
                return;
            }
            ScriptableResourceSite scriptableResourceSite = SampleRandomResourceSite();
            if (scriptableResourceSite == null)
            {
                return;
            }
            ResourceSite resourceSite = Instantiate(_resourceSitePrefab, worldPosition, Quaternion.identity, _resourceSitesContainer);
            resourceSite.Initialize(scriptableResourceSite);
            _resourceSites[position] = resourceSite;
        }

        private ScriptableResourceSite SampleRandomResourceSite()
        {
            float totalWeight = _resourceSiteGenerationDistribution.Sum(pair => pair.Second);
            float randomValue = Random.value * totalWeight;

            foreach (var pair in _resourceSiteGenerationDistribution)
            {
                randomValue -= pair.Second;
                if (randomValue <= 0)
                {
                    return pair.First;
                }
            }

            return null;
        }
    }
}
