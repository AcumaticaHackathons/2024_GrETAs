using System.Collections;
using System.Collections.Generic;
using System.Linq;
using eMission.Extensions.DAC;
using eMission.HTTPClient;
using eMission.Model;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.IN;
using PX.Objects.SO;

namespace eMission.Extensions.BLC
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class GrtSOShipmentEntryExt : PXGraphExtension<SOShipmentEntry>
    {
        /// <summary>
        ///   We want to calculate CO2 cost when the Shipment is created
        /// </summary>
        protected virtual void _(Events.RowInserted<SOShipment> e)
        {
            var rowExt = e.Row?.GetExtension<GRTSOShipmentExt>();
            if (rowExt == null) return;

            rowExt.UsrClimatiqNeedCalculation = true;
        }

        /// <summary>
        ///   When the address ID was changed (override address box was ticked), we want to calculate CO2 cost
        /// </summary>
        protected virtual void _(Events.FieldUpdated<SOShipment.shipAddressID> e)
        {
            var row = (SOShipment)e.Row;
            var rowExt = row?.GetExtension<GRTSOShipmentExt>();
            if (rowExt == null) return;

            rowExt.UsrClimatiqNeedCalculation = true;
        }

        /// <summary>
        ///   If Country or Postal code was changed, we want to calculate CO2 cost
        /// </summary>
        protected virtual void _(Events.RowUpdated<SOShipmentAddress> e)
        {
            if (e.Row == null || Base.Document.Current == null) return;
            var shipmentExt = Base.Document.Current.GetExtension<GRTSOShipmentExt>();

            if (e.Row.PostalCode != e.OldRow.PostalCode || e.Row.CountryID != e.OldRow.CountryID)
            {
                shipmentExt.UsrClimatiqNeedCalculation = true;
            }
        }

        /// <summary>
        ///   CO2 cost calculation runs when we save the shipment
        /// </summary>
        protected void _(Events.RowPersisting<SOShipment> e)
        {
            var rowExt = e.Row?.GetExtension<GRTSOShipmentExt>();
            if (rowExt == null) return;

            if (rowExt.UsrClimatiqNeedCalculation.GetValueOrDefault() && e.Operation != PXDBOperation.Delete)
            {
                calculateCO2CostWorker();
            }
        }

        /// <summary>
        ///   Action for manual CO2 cost calculation
        /// </summary>
        public PXAction<SOShipment> CalculateCO2Cost;

        /// <summary>
        ///   <see cref="CalculateCO2Cost"/>
        /// </summary>
        [PXButton]
        [PXUIField(DisplayName = "Calculate CO2 Cost")]
        public virtual IEnumerable calculateCO2Cost(PXAdapter adapter)
        {
            calculateCO2CostWorker();
            Base.Save.Press();
            return adapter.Get();
        }

        /// <summary>
        ///   This function can be called from an Action or an event handler.
        ///   If we called it from RowPersisting, we don't need to save the changes, but if it was
        ///   called from an Action, we need. <see cref="CalculateCO2Cost"/>
        /// </summary>
        public virtual void calculateCO2CostWorker()
        {
            var shipment = Base.Document.Current;
            if (shipment == null) return;

            var inSite = INSite.PK.Find(Base, shipment.SiteID);
            var siteAddress = Address.PK.Find(Base, inSite.AddressID);
            var destinationAddress = Base.Shipping_Address.Select().RowCast<SOShipmentAddress>()
                                         .FirstOrDefault(it => it.AddressID == shipment.ShipAddressID)
                                     ?? SOAddress.PK.Find(Base, shipment.ShipAddressID);

            if (siteAddress == null || destinationAddress == null) return;

            var shipperAddress = new ClimatiqRequest.Route
            {
                location = new ClimatiqRequest.Location
                {
                    country = siteAddress.CountryID,
                    postal_code = siteAddress.PostalCode
                }
            };

            var shippingModeLand = new ClimatiqRequest.Route
            {
                transport_mode = "road"
            };

            var shippingModeAir = new ClimatiqRequest.Route
            {
                transport_mode = "air"
            };

            var destination = new ClimatiqRequest.Route
            {
                location = new ClimatiqRequest.Location
                {
                    country = destinationAddress.CountryID,
                    postal_code = destinationAddress.PostalCode
                }
            };

            var model = new ClimatiqRequest
            {
                route = new List<ClimatiqRequest.Route>
                {
                    shipperAddress,
                    shippingModeLand,
                    destination
                },
                cargo = new ClimatiqRequest.Cargo
                {
                    weight = shipment.ShipmentWeight.GetValueOrDefault(),
                    weight_unit = "kg"
                }
            };

            var shipmentExt = shipment.GetExtension<GRTSOShipmentExt>();
            var client = new ClimatiqHTTPClient();

            var (co2Cost, error) = GetCO2CostFromClimatiq(client, model);
            shipmentExt.UsrClimateIqLandResult = co2Cost;
            shipmentExt.UsrClimateIqLandError = error;

            model.route = new List<ClimatiqRequest.Route>
            {
                shipperAddress,
                shippingModeLand,
                shippingModeAir,
                shippingModeLand,
                destination
            };

            (co2Cost, error) = GetCO2CostFromClimatiq(client, model);

            shipmentExt.UsrClimateIqAirResult = co2Cost;
            shipmentExt.UsrClimateIqAirError = error;

            Base.Document.Update(shipment);
            
            // Shipping API
            var shipperModel = new ShipperRequest
            {
                carrier_ids = new List<string>
                {
                    "se-6040199" // fedex
                    // "se-6040198"  // ups
                    // "se-6040197"  // usps
                },
                from_country_code = "US",
                from_postal_code = siteAddress.PostalCode.Substring(0, 5),
                from_city_locality = siteAddress.City,
                from_state_province = siteAddress.State,
                to_country_code = "US",
                to_postal_code = destinationAddress.PostalCode.Substring(0, 5),
                to_city_locality = destinationAddress.City,
                to_state_province = destinationAddress.State,
                weight = new ShipperRequest.Weight
                {
                    value = (double)shipment.ShipmentWeight.GetValueOrDefault(),
                    unit = "kilogram"
                },
                dimensions = new ShipperRequest.Dimensions
                {
                    unit = "inch",
                    length = 5.0,
                    width = 5.0,
                    height = 5.0
                },
                confirmation = "none",
                address_residential_indicator = "no"
            };

            var clientShippers = new ShipperHTTPClient();
            var (shippingAmount, shipperError) = GetRateFromShipper(clientShippers, shipperModel);
        }

        private static (decimal?, string) GetCO2CostFromClimatiq(ClimatiqHTTPClient client, ClimatiqRequest model)
        {
            var result = client.CalculateFreightCost(model);
            return ((decimal?)result.QueryResult?.co2e, result.Error);
        }

        private static (ShipperResult.ShippingAmount, string) GetRateFromShipper(ShipperHTTPClient client,
            ShipperRequest model)
        {
            var result = client.GetShipperRates(model);
            return (result.QueryResult?.FirstOrDefault()?.shipping_amount, result.Error);

        }
    }
}