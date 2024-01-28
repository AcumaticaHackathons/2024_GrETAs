using PX.Data;
using PX.Objects.CR.Standalone;


namespace eMission.Extensions.DAC
{
    public sealed class GRTLocationExt : PXCacheExtension<Location>
    {
        public static bool IsActive() { return true; }

        [PXDBInt(MinValue = 0)]
        [PXDefault(0, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "eMission Lead Time (Days)")]
        public int? UsrGRTShipPriority { get; set; }
        public abstract class usrGRTShipPriority : PX.Data.BQL.BqlInt.Field<usrGRTShipPriority> { }


    }
}