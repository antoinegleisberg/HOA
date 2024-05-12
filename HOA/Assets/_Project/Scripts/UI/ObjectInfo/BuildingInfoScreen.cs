using UnityEngine;
using antoinegleisberg.HOA.Core;
using TMPro;
using UnityEngine.UI;

namespace antoinegleisberg.HOA.UI
{
    public class BuildingInfoScreen : MonoBehaviour
    {
        [SerializeField] private ObjectInfoManager _objectInfoManager;

        [SerializeField] private GameObject _buildingInfoFrame;

        [SerializeField] private TextMeshProUGUI _buildingNameText;

        [SerializeField] private Transform _workerInfoContainer;
        [SerializeField] private WorkerInfo _workerInfoPrefab;

        [SerializeField] private Button _seeCitizensButton;
        [SerializeField] private Button _cancelSeeCitizensButton;
        [SerializeField] private Button _previousCitizenButton;
        [SerializeField] private Button _seeCitizenDetailsButton;
        [SerializeField] private Button _nextCitizenButton;

        private int _selectedCitizen = 0;
        private int _nCitizens;

        private Building _building;

        public void SetData(Building building)
        {
            _building = building;

            _buildingNameText.text = building.ScriptableBuilding.Name;

            SetWorkersInfo(building);

            _buildingInfoFrame.SetActive(true);

            _seeCitizensButton.gameObject.SetActive(true);
            _cancelSeeCitizensButton.gameObject.SetActive(false);
            _previousCitizenButton.gameObject.SetActive(false);
            _seeCitizenDetailsButton.gameObject.SetActive(false);
            _nextCitizenButton.gameObject.SetActive(false);

            if (building.IsWorkplace)
            {
                _nCitizens = building.GetComponent<Workplace>().Workers.Count;
            }
            else if (building.IsHouse)
            {
                _nCitizens = building.GetComponent<House>().ResidentsCount;
            }
        }

        public void SeeCitizensAction()
        {
            _buildingInfoFrame.SetActive(false);

            _seeCitizensButton.gameObject.SetActive(false);
            _cancelSeeCitizensButton.gameObject.SetActive(true);
            _previousCitizenButton.gameObject.SetActive(true);
            _seeCitizenDetailsButton.gameObject.SetActive(true);
            _nextCitizenButton.gameObject.SetActive(true);
        }

        public void CancelSeeCitizens()
        {
            _buildingInfoFrame.SetActive(true);

            _seeCitizensButton.gameObject.SetActive(true);
            _cancelSeeCitizensButton.gameObject.SetActive(false);
            _previousCitizenButton.gameObject.SetActive(false);
            _seeCitizenDetailsButton.gameObject.SetActive(false);
            _nextCitizenButton.gameObject.SetActive(false);
        }

        public void SeeNextWorker(int increase)
        {
            _selectedCitizen += increase;
            _selectedCitizen = _selectedCitizen % _nCitizens;
        }

        public void SeeCitizenDetails()
        {
            if (_building.IsWorkplace)
            {
                Citizen citizen = _building.GetComponent<Workplace>().Workers[_selectedCitizen];
                _objectInfoManager.SeeCitizenInfo(citizen);
            }
            else if (_building.IsHouse)
            {
                Citizen citizen = _building.GetComponent<House>().Residents[_selectedCitizen];
                _objectInfoManager.SeeCitizenInfo(citizen);
            }
        }

        private void SetWorkersInfo(Building building)
        {
            foreach (Transform child in _workerInfoContainer.transform)
            {
                Destroy(child.gameObject);
            }

            if (!building.IsWorkplace)
            {
                return;
            }
         
            foreach (Citizen citizen in building.GetComponent<Workplace>().Workers)
            {
                WorkerInfo workerInfo = Instantiate(_workerInfoPrefab, _workerInfoContainer.transform);
                workerInfo.SetData(building.GetComponent<Workplace>(), citizen);
            }
        }
    }
}
