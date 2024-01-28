using System.Collections.Generic;
using PX.Common.Serialization;

namespace eMission.Model
{
    /// <summary>
    ///   Request Model for CO2 cost calculation
    /// </summary>
    [PXSerializable]
    public class ClimatiqRequest
    {
        public List<Route> route { get; set; }
        public Cargo cargo { get; set; }
        
        public class Cargo
        {
            public decimal? weight { get; set; }
            public string weight_unit { get; set; }
        }

        public class LegDetails
        {
            public string vehicle_type { get; set; }
        }

        public class Location
        {
            public string country { get; set; }
            public string postal_code { get; set; }
        }

        public class Route
        {
            public Location location { get; set; }
            public string transport_mode { get; set; }
            public LegDetails leg_details { get; set; }
        }
    }
}