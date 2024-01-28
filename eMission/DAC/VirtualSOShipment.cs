using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.Objects.IN;
using PX.Objects.SO;

namespace eMission.DAC
{
	/// <summary>
	///   Virtual shipment to Store consolidated Shipment Record
	/// </summary>
    public class VirtualSOShipment : IBqlTable
    {
        #region ShipmentType
        public abstract class shipmentType : PX.Data.BQL.BqlString.Field<shipmentType> { }
        protected String _ShipmentType;
        [PXString(1, IsFixed = true)]
        [PXExtraKey]
        [PXDefault(INDocType.Issue)]
        [SOShipmentType.ShortList()]
        [PXUIField(DisplayName = "Type")]
        public virtual string ShipmentType { get; set; }
        #endregion
        
        #region ShipmentNbr
        public abstract class shipmentNbr : PX.Data.BQL.BqlString.Field<shipmentNbr> { }
        [PXString(15, IsKey = true, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDefault]
        [PXUIField(DisplayName = "Shipment Nbr.", Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search2<SOShipment.shipmentNbr,
            InnerJoin<INSite, On<SOShipment.FK.Site>,
                LeftJoinSingleTable<Customer, On<SOShipment.customerID, Equal<Customer.bAccountID>>>>,
            Where2<Match<INSite, Current<AccessInfo.userName>>,
                And<Where<Customer.bAccountID, IsNull, Or<Match<Customer, Current<AccessInfo.userName>>>>>>,
            OrderBy<Desc<SOShipment.shipmentNbr>>>))]
        [PX.Data.EP.PXFieldDescription]
        public virtual string ShipmentNbr { get; set; }
        #endregion    
        
        #region Status
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        protected string _Status;
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        [SOShipmentStatus.List]
        [PXDefault]
        public virtual string Status { get; set; }
        #endregion
        
        #region ShipDate
        public abstract class shipDate : PX.Data.BQL.BqlDateTime.Field<shipDate> { }
        protected DateTime? _ShipDate;
        [PXDate]
        [PXUIField(DisplayName = "Shipment Date", Visibility = PXUIVisibility.SelectorVisible)]
        [PXDefault(typeof(AccessInfo.businessDate))]
        public virtual DateTime? ShipDate { get; set; }
        #endregion
        
	    #region CustomerID
		public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
		[PXInt]
		[PXUIField(DisplayName = "Customer", IsReadOnly = true)]
		[PXSelector(
			typeof(Search<BAccountR.bAccountID, Where<True, Equal<True>>>), 
			DescriptionField = typeof(Customer.acctCD), 
			Filterable = true)]
		public virtual int? CustomerID { get; set; }
		#endregion
		
		#region CustomerLocationID
		public abstract class customerLocationID : PX.Data.BQL.BqlInt.Field<customerLocationID> { }
		[PXInt]
		[PXUIField(DisplayName = "Location", IsReadOnly = true)]
		[PXSelector(typeof(SelectFrom<Location>.SearchFor<Location.locationID>),
			DescriptionField = typeof(Location.locationCD))]	
		public virtual int? CustomerLocationID { get; set; }
		#endregion
		
		#region SiteID
		public abstract class siteID : PX.Data.BQL.BqlInt.Field<siteID> { }
		[PXInt]
		[PXUIField(DisplayName = "Warehouse ID")]
		[PXSelector(typeof(SelectFrom<INSite>.SearchFor<INSite.siteID>), DescriptionField = typeof(INSite.descr))]
		public virtual int? SiteID { get; set; }
		#endregion
		
		#region ShipAddressID
		/// <summary>
		///   Ship Address ID
		/// </summary>
		[PXInt]
		[PXUIField(DisplayName="Ship Address ID")]
		public int? ShipAddressID { get; set; }
		/// <exclude/>
		public abstract class shipAddressID : PX.Data.BQL.BqlInt.Field<shipAddressID> { }
		#endregion
        
		#region ShipmentQty
		public abstract class shipmentQty : PX.Data.BQL.BqlDecimal.Field<shipmentQty> { }
		[PXQuantity]
		[PXUnboundDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Shipped Quantity", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual decimal? ShipmentQty { get; set; }
		#endregion
		
		#region ShipmentWeight
		public abstract class shipmentWeight : PX.Data.BQL.BqlDecimal.Field<shipmentWeight> { }
		[PXDecimal(6)]
		[PXUnboundDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Shipped Weight", Enabled = false)]
		public virtual decimal? ShipmentWeight { get; set; }
		#endregion
		
		[PXDecimal(6)]
		[PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "ClimateIq CO2 Amount - Land", Enabled = false, IsReadOnly = true)]
		public Decimal? UsrClimateIqLandResult { get; set; }
		public abstract class usrClimateIqLandResult : PX.Data.BQL.BqlDecimal.Field<usrClimateIqLandResult> { }

		[PXDecimal(6)]
		[PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "ClimateIq CO2 Amount - Air", Enabled = false, IsReadOnly = true)]
		public Decimal? UsrClimateIqAirResult { get; set; }
		public abstract class usrClimateIqAirResult : PX.Data.BQL.BqlDecimal.Field<usrClimateIqAirResult> { }

		[PXDecimal(6)]
		[PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "ClimateIq CO2 Amount - Land - Before", Enabled = false, IsReadOnly = true)]
		public decimal? UsrClimateIqLandResultBeforeConsolidation { get; set; }
		public abstract class usrClimateIqLandResultBeforeConsolidation : PX.Data.BQL.BqlDecimal.Field<usrClimateIqLandResultBeforeConsolidation> { }

		[PXDecimal(6)]
		[PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "ClimateIq CO2 Amount - Air - Before", Enabled = false, IsReadOnly = true)]
		public Decimal? UsrClimateIqAirResultBeforeConsolidation { get; set; }
		public abstract class usrClimateIqAirResultBeforeConsolidation : PX.Data.BQL.BqlDecimal.Field<usrClimateIqAirResultBeforeConsolidation> { }

    }
}