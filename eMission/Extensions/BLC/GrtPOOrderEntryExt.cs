using System.Collections;
using System.Collections.Generic;
using System.Linq;
using eMission.Extensions.DAC;
using eMission.HTTPClient;
using eMission.Model;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.IN;
using PX.Objects.PO;

namespace eMission.Extensions.BLC
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class GrtPOOrderEntryExt : PXGraphExtension<POOrderEntry>
    {
        /// <summary>
        ///   We want to calculate CO2 cost when the Shipment is created
        /// </summary>
        protected virtual void _(Events.RowInserted<POOrder> e)
        {
            var rowExt = e.Row?.GetExtension<GRTPOOrderExt>();
            if (rowExt == null) return;

            rowExt.UsrClimatiqNeedCalculation = true;
        }

        /// <summary>
        ///   When the address ID was changed (override address box was ticked), we want to calculate CO2 cost
        /// </summary>
        protected virtual void _(Events.FieldUpdated<POOrder.shipAddressID> e)
        {
            var row = (POOrder)e.Row;
            var rowExt = row?.GetExtension<GRTPOOrderExt>();
            if (rowExt == null) return;

            rowExt.UsrClimatiqNeedCalculation = true;
        }

        /// <summary>
        ///   If Country or Postal code was changed, we want to calculate CO2 cost
        /// </summary>
        protected virtual void _(Events.RowUpdated<POAddress> e)
        {
            if (e.Row == null || Base.Document.Current == null) return;
            var shipmentExt = Base.Document.Current.GetExtension<GRTPOOrderExt>();

            if (e.Row.PostalCode != e.OldRow.PostalCode || e.Row.CountryID != e.OldRow.CountryID)
            {
                shipmentExt.UsrClimatiqNeedCalculation = true;
            }
        }

        /// <summary>
        ///   CO2 cost calculation runs when we save the shipment
        /// </summary>
        protected void _(Events.RowPersisting<POOrder> e)
        {
            var rowExt = e.Row?.GetExtension<GRTPOOrderExt>();
            if (rowExt == null) return;

            if (rowExt.UsrClimatiqNeedCalculation.GetValueOrDefault() && e.Operation != PXDBOperation.Delete)
            {
                calculateCO2CostWorker();
            }
        }

        /// <summary>
        ///   Action for manual CO2 cost calculation
        /// </summary>
        public PXAction<POOrder> CalculateCO2Cost;

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
            var poOrder = Base.Document.Current;
            if (poOrder == null) return;

            //  var inSite = INSite.PK.Find(Base, poOrder.SiteID);
            var siteAddress = PORemitAddress.PK.Find(Base, poOrder.RemitAddressID);
            var destinationAddress = Base.Shipping_Address.Select().RowCast<POShipAddress>()
                .FirstOrDefault(it => it.AddressID == poOrder.ShipAddressID)
                                     ?? POShipAddress.PK.Find(Base, poOrder.ShipAddressID);

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
                    weight = poOrder.OrderWeight.GetValueOrDefault(),
                    weight_unit = "kg"
                }
            };

            var shipmentExt = poOrder.GetExtension<GRTPOOrderExt>();
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
        }

        private static (decimal?, string) GetCO2CostFromClimatiq(ClimatiqHTTPClient client, ClimatiqRequest model)
        {
            var result = client.CalculateFreightCost(model);
            return ((decimal?)result.QueryResult?.co2e, result.Error);
        }
    }
}