using System.Collections.Generic;
using PX.Common.Serialization;

namespace eMission.Model
{
    /// <summary>
    ///   Request Model for CO2 cost calculation
    /// </summary>
    [PXSerializable]
    public class ShipperRequest
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Dimensions
        {
            public string unit { get; set; }
            public double length { get; set; }
            public double width { get; set; }
            public double height { get; set; }
        }

        //public class Root
        //{
            public List<string> carrier_ids { get; set; }
            public string from_country_code { get; set; }
            public string from_postal_code { get; set; }
            public string from_city_locality { get; set; }
            public string from_state_province { get; set; }
            public string to_country_code { get; set; }
            public string to_postal_code { get; set; }
            public string to_city_locality { get; set; }
            public string to_state_province { get; set; }
            public Weight weight { get; set; }
            public Dimensions dimensions { get; set; }
            public string confirmation { get; set; }
            public string address_residential_indicator { get; set; }
        //}

        public class Weight
        {
            public double value { get; set; }
            public string unit { get; set; }
        }

    }
}