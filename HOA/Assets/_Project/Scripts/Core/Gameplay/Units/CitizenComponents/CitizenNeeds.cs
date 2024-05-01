using UnityEngine;


namespace antoinegleisberg.HOA
{
    public class CitizenNeeds : MonoBehaviour
    {
        // Basic needs
        public int Hunger { get { return Hunger; } set { Hunger = value; if (value < 0) Hunger = 0; } }
        public int Thirst { get { return Thirst; } set { Thirst = value; if (value < 0) Thirst = 0; } }
        public int Rest { get { return Rest; } set { Rest = value; if (value < 0) Rest = 0; } }  // Describes how well rested the citizen is
        public int HousingSatisfaction { get { return HousingSatisfaction; } set { HousingSatisfaction = value; if (value < 0) HousingSatisfaction = 0; } }  // Both access to housing and quality of housing - link to houses that can crumble / catch fire

        // Community needs
        public int MarketplaceAccess { get; private set; }
        public int Religion { get; private set; }
        public int PoliticalSatisfaction { get; private set; }  // How well the citizen thinks the city is run - depends on taxes, etc.
        public int CommunityAccess { get; private set; }  // Access to community buildings like taverns, parks, festivals, etc.

        // Education needs
        public int Education { get; private set; }  // Boosts certain work tasks ??? - through school + libraries
        public int WorkExperience { get; private set; }  // Boosts work productivity

        // Entertainment needs
        public int EntertainmentAccess { get; private set; }  // sports, movies, etc.
        public int CulturalAccess { get; private set; }  // Access to cultural buildings like theaters, museums, etc.

        // Health needs
        public int Health { get; private set; }
        public bool IsSick { get; private set; }

        // Safety needs
        public int Safety { get; private set; }  //Use only if I implement military, bandits, etc.

        // Luxury needs
        public int Clothing { get; private set; }
        public int Jewelry { get; private set; }
        public int Decoration { get; private set; }  // Describes how well decorated the city is
    }
}
