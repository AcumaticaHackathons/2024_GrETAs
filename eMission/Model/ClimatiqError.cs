using PX.Common.Serialization;

namespace eMission.Model
{
    [PXSerializable]
    public class ClimatiqError
    {
        public string error { get; set; }
        
        public string error_code { get; set; }
        
        public string message { get; set; }
    }
}