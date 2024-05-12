using System;
using UnityEngine;
using antoinegleisberg.HOA.Core;
using antoinegleisberg.HOA.EventSystem;

namespace antoinegleisberg.HOA.UI
{
    public class ObjectInfoManager : MonoBehaviour
    {
        [SerializeField] private Canvas _objectInfoCanvas;
        
        [SerializeField] private CitizenInfoScreen _citizenInfoScreen;
        [SerializeField] private GameObject _citizenInfoFrame;
        
        [SerializeField] private BuildingInfoScreen _buildingInfoScreen;
        [SerializeField] private GameObject _buildingInfoFrame;

        private Collider2D _currentCollider = null;

        private Action<Collider2D, Collider2D> OnSecondColliderClicked = null;

        private void Start()
        {
            UIEvents.Instance.OnCloseObjectInfo += () => _currentCollider = null;
        }

        private void OnDestroy()
        {
            UIEvents.Instance.OnCloseObjectInfo -= () => _currentCollider = null;
        }

        public void OnColliderClicked(Collider2D collider)
        {
            if (collider == null && _currentCollider == null)
            {
                return;
            }
            else if (_currentCollider == null)
            {
                _currentCollider = collider;
                OpenObjectInfo();
            }
            else if (collider == null)
            {
                // clicked nothing: cancel current action
                OpenObjectInfo();
            }
            else
            {
                OnSecondColliderClicked?.Invoke(_currentCollider, collider);
                UIEvents.Instance.CloseObjectInfo();
            }
        }

        public void SetReassignCitizenHomeAction()
        {
            OnSecondColliderClicked = ReassignCitizenHome;
            _citizenInfoFrame.SetActive(false);
        }

        public void SetReassignCitizenWorkplaceAction()
        {
            OnSecondColliderClicked = ReassignCitizenWorkplace;
            _citizenInfoFrame.SetActive(false);
        }

        public void SeeCitizenHomeAction()
        {
            if (_currentCollider.TryGetComponent(out Citizen citizen))
            {
                if (citizen.Home != null)
                {
                    _currentCollider = citizen.Home.GetComponent<Collider2D>();
                    OpenObjectInfo();
                }
            }
        }

        public void SeeCitizenWorkplaceAction()
        {
            if (_currentCollider.TryGetComponent(out Citizen citizen))
            {
                if (citizen.Workplace != null)
                {
                    _currentCollider = citizen.Workplace.GetComponent<Collider2D>();
                    OpenObjectInfo();
                }
            }
        }

        public void SeeCitizenInfo(Citizen citizen)
        {
            _currentCollider = citizen.GetComponent<Collider2D>();
            OpenObjectInfo();
        }

        private void ReassignCitizenHome(Collider2D first, Collider2D second)
        {
            if (first.TryGetComponent(out Citizen citizen))
            {
                if (second.TryGetComponent(out House house))
                {
                    citizen.ClaimHouse(house);
                }
            }
        }

        private void ReassignCitizenWorkplace(Collider2D first, Collider2D second)
        {
            if (first.TryGetComponent(out Citizen citizen))
            {
                if (second.TryGetComponent(out Workplace workplace))
                {
                    citizen.ClaimWorkplace(workplace);
                }
            }
        }

        private void OpenObjectInfo()
        {
            _citizenInfoScreen.gameObject.SetActive(false);
            _buildingInfoScreen.gameObject.SetActive(false);

            if (_currentCollider.TryGetComponent(out Citizen citizen))
            {
                _citizenInfoScreen.SetData(citizen);
                _citizenInfoScreen.gameObject.SetActive(true);
                _citizenInfoFrame.SetActive(true);
            }

            if (_currentCollider.TryGetComponent(out Building building))
            {
                _buildingInfoScreen.SetData(building);
                _buildingInfoScreen.gameObject.SetActive(true);
            }
        }
    }
}
