using antoinegleisberg.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public partial class UnitManager
    {
        private struct CitizensSaveData
        {
            public List<CitizenSaveData> Citizens;
        }

        
        private struct CitizenSaveData
        {
            public string HouseGuid;
            public string WorkplaceGuid;
            public float[] Position;
            public bool IsInBuilding;
            public string CurrentBuildingGuid;
            public string CurrentState;
            public string CurrentRecipeName;

            public CitizenSaveData(Citizen citizen)
            {
                Position = new float[3] { citizen.transform.position.x, citizen.transform.position.y, citizen.transform.position.z };
                CurrentRecipeName = "";

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
                    if (citizen.Workplace.GetComponent<ProductionSite>() != null)
                    {
                        CurrentRecipeName = citizen.Workplace.GetComponent<ProductionSite>().GetRecipe(citizen).Name;
                    }
                }
                else
                {
                    WorkplaceGuid = "";
                }

                IsInBuilding = citizen.IsInBuilding;
                if (citizen.CurrentBuilding != null)
                {
                    CurrentBuildingGuid = citizen.CurrentBuilding.GetComponent<GuidHolder>().UniqueId;
                }
                else
                {
                    CurrentBuildingGuid = "";
                }
                CurrentState = citizen.GetCurrentStateAsString();
            }
        }
    }
}
