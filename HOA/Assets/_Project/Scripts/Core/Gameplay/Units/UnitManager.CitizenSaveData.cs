using antoinegleisberg.Saving;
using System.Collections.Generic;

namespace antoinegleisberg.HOA
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
