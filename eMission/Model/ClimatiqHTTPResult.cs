using PX.Common.Serialization;

namespace eMission.Model
{
    /// <summary>
    ///   Generic result object of HTTP client.
    ///   If there is no error, QueryResult is getting populated, otherwise only Error will have a value
    /// </summary>
    [PXSerializable]
    public class ClimatiqHTTPResult
    {
        public ClimatiqResult QueryResult { get; set; }
        
        public string Error { get; set; }
    }
}