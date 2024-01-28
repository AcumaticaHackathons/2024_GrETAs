using System;
using eMission.Extensions.BLC;
using PX.Data;
using PX.Objects.SO;

namespace eMission.Extensions.DAC
{
    public sealed class GRTSOShipmentExt : PXCacheExtension<SOShipment>
    {
        public static bool IsActive() { return true; }
        
		[PXDBDecimal(6)]
		[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "ClimateIq CO2 Amount - Land", Enabled = false, IsReadOnly = true)]
		public Decimal? UsrClimateIqLandResult { get; set; }
		public abstract class usrClimateIqLandResult : PX.Data.BQL.BqlDecimal.Field<usrClimateIqLandResult> { }

		#region UsrClimateIqLandError
		/// <summary>
		///   Error - Land
		/// </summary>
		[PXDBString]
		[PXUIField(DisplayName="ClimateIq Error - Land", IsReadOnly = true)]
		[PXUIVisible(typeof(Where<usrClimateIqLandError, IsNotNull>))]
		public string UsrClimateIqLandError { get; set; }
		/// <exclude/>
		public abstract class usrClimateIqLandError : PX.Data.BQL.BqlString.Field<usrClimateIqLandError> { }
		#endregion
        
		[PXDBDecimal(6)]
		[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "ClimateIq CO2 Amount - Air", Enabled = false, IsReadOnly = true)]
		public Decimal? UsrClimateIqAirResult { get; set; }
		public abstract class usrClimateIqAirResult : PX.Data.BQL.BqlDecimal.Field<usrClimateIqAirResult> { }

		#region UsrClimateIqAirError	
		/// <summary>
		///   Error - Air
		/// </summary>
		[PXDBString]
		[PXUIField(DisplayName="ClimateIq Error - Air", IsReadOnly = true)]
		[PXUIVisible(typeof(Where<usrClimateIqAirError, IsNotNull>))]
		public string UsrClimateIqAirError { get; set; }
		/// <exclude/>
		public abstract class usrClimateIqAirError : PX.Data.BQL.BqlString.Field<usrClimateIqAirError> { }
		#endregion
		
		#region UsrClimatiqNeedCalculation
		/// <summary>
		///   Need CO2 Calculation field
		///   This flag is used in RowPersisting in <see cref="GrtSOShipmentEntryExt"/>
		/// </summary>
		[PXBool]
		[PXUIField(DisplayName="Need Calculation")]
		public bool? UsrClimatiqNeedCalculation { get; set; }
		/// <exclude/>
		public abstract class usrClimatiqNeedCalculation : PX.Data.BQL.BqlBool.Field<usrClimatiqNeedCalculation> { }
		#endregion

        [PXString]
        [PXUIField(DisplayName = "Shipper Info")]
        public string UsrShipperInfo { get; set; }
        /// <exclude/>
        public abstract class usrShipperInfo : PX.Data.BQL.BqlString.Field<usrShipperInfo> { }

	}
}