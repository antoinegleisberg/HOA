using antoinegleisberg.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public partial class UnitManager : MonoBehaviour, ISaveable
    {
        public static UnitManager Instance { get; private set; }

        [SerializeField] private Transform _citizensContainer;
        [SerializeField] private Citizen _citizenPrefab;

        private List<Citizen> _citizens;

        private void Awake()
        {
            Instance = this;
            _citizens = new List<Citizen>();
        }
        
        public Citizen SpawnCitizen(Vector3 position)
        {
            Citizen citizen = Instantiate(_citizenPrefab, position, Quaternion.identity, _citizensContainer);
            _citizens.Add(citizen);
            return citizen;
        }

        public void LoadData(object data)
        {
            CitizensSaveData citizensSaveData = (CitizensSaveData)data;

            foreach (CitizenSaveData citizenSaveData in citizensSaveData.Citizens)
            {
                StartCoroutine(SpawnCitizenCoroutine(citizenSaveData));
            }
        }

        public object GetSaveData()
        {
            CitizensSaveData saveData = new CitizensSaveData()
            {
                Citizens = new List<CitizenSaveData>(_citizens.Count)
            };
            foreach (Citizen citizen in _citizens)
            {
                saveData.Citizens.Add(new CitizenSaveData(citizen));
            }

            return saveData;
        }

        private IEnumerator SpawnCitizenCoroutine(CitizenSaveData citizenSaveData)
        {
            Workplace workplace = null;
            House house = house = null;
            if (!string.IsNullOrEmpty(citizenSaveData.HouseGuid))
            {
                yield return new WaitUntil(() => FindObjectsOfType<GuidHolder>().Where(t => t.UniqueId == citizenSaveData.HouseGuid).First() != null);
                house = FindObjectsOfType<GuidHolder>().Where(t => t.UniqueId == citizenSaveData.HouseGuid).First().GetComponent<House>();
            }

            if (!string.IsNullOrEmpty(citizenSaveData.WorkplaceGuid))
            {
                yield return new WaitUntil(() => FindObjectsOfType<GuidHolder>().Where(t => t.UniqueId == citizenSaveData.WorkplaceGuid).First() != null);
                workplace = FindObjectsOfType<GuidHolder>().Where(t => t.UniqueId == citizenSaveData.WorkplaceGuid).First().GetComponent<Workplace>();
            }

            Vector3 spawnPosition = new Vector3(citizenSaveData.Position[0], citizenSaveData.Position[1], citizenSaveData.Position[2]);

            Building currentBuilding = null;
            if (citizenSaveData.IsInBuilding && !string.IsNullOrEmpty(citizenSaveData.CurrentBuildingGuid))
            {
                yield return new WaitUntil(() => FindObjectsOfType<GuidHolder>().Where(t => t.UniqueId == citizenSaveData.CurrentBuildingGuid).First() != null);
                currentBuilding = FindObjectsOfType<GuidHolder>().Where(t => t.UniqueId == citizenSaveData.CurrentBuildingGuid).First().GetComponent<Building>();
                Debug.Log("Setting current building");
            }
            else
            {
                // If the citizen is not in a building, snap its position to the closest node
                spawnPosition = PathfindingGraph.Instance.GetClosestNodeCoordinates(spawnPosition);
            }

            Citizen citizen = SpawnCitizen(spawnPosition);
            if (house != null)
            {
                citizen.ClaimHouse(house);
            }
            if (workplace != null)
            {
                citizen.ClaimWorkplace(workplace);
            }
            if (currentBuilding != null)
            {
                citizen.SetCurrentBuilding(currentBuilding);
            }
            if (!string.IsNullOrEmpty(citizenSaveData.CurrentRecipeName))
            {
                citizen.Workplace.GetComponent<ProductionSite>().SetRecipe(citizen, RecipesDB.GetRecipeByName(citizenSaveData.CurrentRecipeName));
            }

            citizen.SetStateFromString(citizenSaveData.CurrentState);
        }
    }
}
