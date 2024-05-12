using UnityEngine;


namespace antoinegleisberg.HOA.Core
{
    public class CitizenNeeds : MonoBehaviour
    {
        // For all needs, a higher value describes a high satisfaction of that need
        
        // Basic needs
        private int _hunger;
        public int Hunger { get { return _hunger; } set { _hunger = value; if (value < 0) _hunger = 0; } }
        
        private int _thirst;
        public int Thirst { get { return _thirst; } set { _thirst = value; if (value < 0) _thirst = 0; } }
        
        private int _rest;
        public int Rest { get { return _rest; } set { _rest = value; if (value < 0) _rest = 0; } }

        // Describes both access to housing and quality of housing - link to houses that can crumble / catch fire
        private int _housingSatisfaction;
        public int HousingSatisfaction { get { return _housingSatisfaction; } set { _housingSatisfaction = value; if (value < 0) _housingSatisfaction = 0; } }
        

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
