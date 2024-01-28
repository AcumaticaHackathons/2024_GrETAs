using System.Collections;
using System.Collections.Generic;
using System.Linq;
using eMission.Cache;
using eMission.DAC;
using eMission.Extensions.DAC;
using eMission.HTTPClient;
using eMission.Model;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.DependencyInjection;
using PX.Objects.CR;
using PX.Objects.IN;
using PX.Objects.SO;

namespace eMission.Graph
{
    /// <summary>
    ///   Consolidate Shipment Processing form
    /// </summary>
    public class GRTConsolidateShipment : PXGraph<GRTConsolidateShipment>, IGraphWithInitialization
    {
        public PXSave<SOShipment> Save;
        public PXCancel<SOShipment> Cancel;
        
        public SelectFrom<SOShipment>
            .Where<SOShipment.status.IsEqual<SOShipmentStatus.open>>
            .ProcessingView OpenShipments;
        
        public SelectFrom<VirtualSOShipment>.View.ReadOnly ConsolidatedShipments;

        public IEnumerable consolidatedShipments()
        {
            return ConsolidatedShipments.Cache.Cached.Cast<VirtualSOShipment>();
        }
        
        public virtual void Initialize()
        {
            Caches[typeof(VirtualSOShipment)] = new PermanentCache<VirtualSOShipment>(this);
            OpenShipments.SetProcessDelegate(ProcessShipments);
        }
        
        private void ProcessShipments(List<SOShipment> shipmentList)
        {
            foreach (VirtualSOShipment shipment in ConsolidatedShipments.Cache.Cached)
            {
                ConsolidatedShipments.Delete(shipment);
            }
                
            var shipmentGroup = shipmentList
                .GroupBy(it => new
                {
                    it.ShipmentType,
                    it.Status,
                    it.CustomerID,
                    it.CustomerLocationID,
                    it.SiteID,
                    it.ShipAddressID
                })
                .ToList();

            foreach (var group in shipmentGroup)
            {
                var shipment = ConsolidatedShipments.Insert(new VirtualSOShipment
                {
                    ShipmentType = group.Key.ShipmentType,
                    ShipmentNbr = group.First().ShipmentNbr,
                    Status = group.Key.Status,
                    ShipDate = group.Last().ShipDate,
                    CustomerID = group.Key.CustomerID,
                    SiteID = group.Key.SiteID,
                    ShipAddressID = group.Key.ShipAddressID,
                    CustomerLocationID = group.Key.CustomerLocationID
                });
                    
                foreach (var row in group)
                {
                    var rowExt = row.GetExtension<GRTSOShipmentExt>();
                    shipment.ShipmentQty += row.ShipmentQty;
                    shipment.ShipmentWeight += row.ShipmentWeight;
                    shipment.UsrClimateIqAirResultBeforeConsolidation += rowExt.UsrClimateIqAirResult;
                    shipment.UsrClimateIqLandResultBeforeConsolidation += rowExt.UsrClimateIqLandResult;
                }

                calculateCO2Cost(shipment);
                ConsolidatedShipments.Update(shipment);
            }
        }
        
        public virtual void calculateCO2Cost(VirtualSOShipment shipment)
        {
            var inSite = INSite.PK.Find(this, shipment.SiteID);
            var siteAddress = Address.PK.Find(this, inSite.AddressID);
            var destinationAddress = SOAddress.PK.Find(this, shipment.ShipAddressID);
            
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
            
            var client = new ClimatiqHTTPClient();
            
            var co2Cost = GetCO2CostFromClimatiq(client, model);
            shipment.UsrClimateIqLandResult = co2Cost;
            
            model.route = new List<ClimatiqRequest.Route>
            {
                shipperAddress,
                shippingModeLand,
                shippingModeAir,
                shippingModeLand,
                destination
            };
            
            co2Cost = GetCO2CostFromClimatiq(client, model);
            
            shipment.UsrClimateIqAirResult = co2Cost;
        }

        private static decimal? GetCO2CostFromClimatiq(ClimatiqHTTPClient client, ClimatiqRequest model)
        {
            var result = client.CalculateFreightCost(model);
            return (decimal?)result.QueryResult?.co2e;
        }
    }
}