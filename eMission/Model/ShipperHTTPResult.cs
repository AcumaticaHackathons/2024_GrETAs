using PX.Common.Serialization;
using System.Collections.Generic;

namespace eMission.Model
{
    /// <summary>
    ///   Generic result object of HTTP client.
    ///   If there is no error, QueryResult is getting populated, otherwise only Error will have a value
    /// </summary>
    [PXSerializable]
    public class ShipperHTTPResult
    {
        public List<ShipperResult> QueryResult { get; set; }
        
        public string Error { get; set; }
    }
}