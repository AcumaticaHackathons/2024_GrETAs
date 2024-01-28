using PX.Common.Serialization;

namespace eMission.Model
{
    [PXSerializable]
    public class ShipperError
    {
        public string error { get; set; }
        
        public string error_code { get; set; }
        
        public string message { get; set; }
    }
}