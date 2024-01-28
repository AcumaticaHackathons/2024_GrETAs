using System.Collections.Generic;
using PX.Common.Serialization;

namespace eMission.Model
{
    /// <summary>
    ///   Result model of CO2 cost calculation call
    /// </summary>
    [PXSerializable]
    public class ClimatiqResult
    {
        public double? co2e { get; set; }
        public string co2e_unit { get; set; }
        public double? distance_km { get; set; }
        public List<Route> route { get; set; }
        public List<object> notices { get; set; }
        
        public class ActivityData
        {
            public double? activity_value { get; set; }
            public string activity_unit { get; set; }
        }

        public class Estimate
        {
            public double? co2e { get; set; }
            public string co2e_unit { get; set; }
            public string co2e_calculation_method { get; set; }
            public string co2e_calculation_origin { get; set; }
            public object emission_factor { get; set; }
            public object constituent_gases { get; set; }
            public ActivityData activity_data { get; set; }
            public string audit_trail { get; set; }
        }
        
        public class Route
        {
            public string type { get; set; }
            public string name { get; set; }
            public double? confidence_score { get; set; }
            public double? co2e { get; set; }
            public string co2e_unit { get; set; }
            public string transport_mode { get; set; }
            public double? distance_km { get; set; }
            public List<Estimate> estimates { get; set; }
        }
    }
}