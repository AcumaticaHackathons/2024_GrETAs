using PX.Data;
using PX.Objects.SO;

namespace eMission.Extensions.DAC
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public sealed class GRTSOSetupExt : PXCacheExtension<SOSetup>
    {
        public static bool IsActive() { return true; }

        #region UsrGRTClimatiqUrt
        /// <summary>
        ///   Url
        /// </summary>
        [PXDBString]
        [PXUIField(DisplayName="Url")]
        public string UsrGRTClimatiqUrl { get; set; }
        /// <exclude/>
        public abstract class usrGRTClimatiqUrl : PX.Data.BQL.BqlString.Field<usrGRTClimatiqUrl> { }
        #endregion
        
        #region UsrGRTClimatiqAPIKey
        /// <summary>
        ///   API Key field
        /// </summary>
        [PXDBString(50)]
        [PXUIField(DisplayName="API Key")]
        public string UsrGRTClimatiqAPIKey { get; set; }
        /// <exclude/>
        public abstract class usrGRTClimatiqAPIKey : PX.Data.BQL.BqlString.Field<usrGRTClimatiqAPIKey> { }
        #endregion

        #region UsrGRTEnableTracking
        /// <summary>
        ///   Enable Customization flag
        /// </summary>
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Enable Emission Tracking")]
        public bool? UsrGRTEnableTracking { get; set; }
        /// <exclude/>
        public abstract class usrGRTEnableTracking : PX.Data.BQL.BqlBool.Field<usrGRTEnableTracking> { }


        #endregion
    }
}