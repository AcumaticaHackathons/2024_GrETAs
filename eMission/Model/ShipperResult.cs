using System;
using System.Collections.Generic;
using PX.Common.Serialization;

namespace eMission.Model
{
    /// <summary>
    ///   Result model of CO2 cost calculation call
    /// </summary>
    [PXSerializable]
    public class ShipperResult
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        public class ConfirmationAmount
        {
            public string currency { get; set; }
            public double amount { get; set; }
        }

        public class InsuranceAmount
        {
            public string currency { get; set; }
            public double amount { get; set; }
        }

        public class OtherAmount
        {
            public string currency { get; set; }
            public double amount { get; set; }
        }

        public class RequestedComparisonAmount
        {
            public string currency { get; set; }
            public double amount { get; set; }
        }

        //public class Root
        //{
            public string rate_type { get; set; }
            public string carrier_id { get; set; }
            public ShippingAmount shipping_amount { get; set; }
            public InsuranceAmount insurance_amount { get; set; }
            public ConfirmationAmount confirmation_amount { get; set; }
            public OtherAmount other_amount { get; set; }
            public RequestedComparisonAmount requested_comparison_amount { get; set; }
            public List<object> rate_details { get; set; }
            public object zone { get; set; }
            public object package_type { get; set; }
            public int delivery_days { get; set; }
            public bool guaranteed_service { get; set; }
            public DateTime estimated_delivery_date { get; set; }
            public string carrier_delivery_days { get; set; }
            public DateTime ship_date { get; set; }
            public bool negotiated_rate { get; set; }
            public string service_type { get; set; }
            public string service_code { get; set; }
            public bool trackable { get; set; }
            public string carrier_code { get; set; }
            public string carrier_nickname { get; set; }
            public string carrier_friendly_name { get; set; }
            public string validation_status { get; set; }
            public List<object> warning_messages { get; set; }
            public List<object> error_messages { get; set; }
       // }

        public class ShippingAmount
        {
            public string currency { get; set; }
            public double amount { get; set; }
        }


    }
}