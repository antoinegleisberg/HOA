using antoinegleisberg.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class UnitManager : MonoBehaviour, ISaveable
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

            Vector3 spawnPosition = house == null ? Vector3.zero : house.transform.position;

            Citizen citizen = Instantiate(_citizenPrefab, spawnPosition, Quaternion.identity, _citizensContainer);
            if (house != null)
            {
                citizen.ClaimHouse(house);
            }
            if (workplace != null)
            {
                citizen.ClaimWorkplace(workplace);
            }
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
                Citizens = new List<CitizenSaveData>(),
            };
            foreach (Citizen citizen in _citizens)
            {
                saveData.Citizens.Add(new CitizenSaveData(citizen));
            }

            return saveData;
        }

        private struct CitizensSaveData
        {
            public List<CitizenSaveData> Citizens;
        }

        private struct CitizenSaveData
        {
            public string HouseGuid;
            public string WorkplaceGuid;

            public CitizenSaveData(Citizen citizen)
            {
                if (citizen.Home != null)
                {
                    HouseGuid = citizen.Home.GetComponent<GuidHolder>().UniqueId;
                }
                else
                {
                    HouseGuid = "";
                }

                if (citizen.Workplace != null)
                {
                    WorkplaceGuid = citizen.Workplace.GetComponent<GuidHolder>().UniqueId;
                }
                else
                {
                    WorkplaceGuid = "";
                }
            }
        }
    }
}
