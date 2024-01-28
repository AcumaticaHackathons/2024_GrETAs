using eMission.Extensions.DAC;
using PX.Data;
using PX.Objects.SO;

namespace eMission.Cache
{
    /// <summary>
    ///   Climatiq settings cache for authentication.
    /// </summary>
    public class ClimatiqSettings : IPrefetchable
    {
        /// <summary>
        ///   API Key
        /// </summary>
        private string _apiKey;

        /// <summary>
        ///   Url
        /// </summary>
        private string _url;

        /// <summary>
        ///   eMissionConfig
        /// </summary>
        private bool? _eMissionConfig;

        /// <summary>
        ///   Get Climatiq Settings
        /// </summary>
        public static (string APIKey, string Url) GetSettings()
        {
            var settings = PXDatabase.GetSlot<ClimatiqSettings>("Climatiq");
            return (settings._apiKey, settings._url);
        }

        /// <summary>
        ///   Get eMission Flag Key  
        /// </summary>
        public static bool? GetConfigFlag()
        {
            var pimToken = PXDatabase.GetSlot<ClimatiqSettings>("Climatiq");
            return pimToken._eMissionConfig;
        }

        /// <summary>
        ///   Prefetch here just initialize the API Key
        /// </summary>
        public void Prefetch()
        {
            var setupRecord = PXDatabase.SelectSingle<SOSetup>(
                new PXDataField(nameof(GRTSOSetupExt.usrGRTClimatiqAPIKey), "SOSetup"),
                new PXDataField(nameof(GRTSOSetupExt.UsrGRTClimatiqUrl), "SOSetup"),
                new PXDataField(nameof(GRTSOSetupExt.UsrGRTEnableTracking), "SOSetup"));

            _apiKey = setupRecord.GetString(0);
            _url = setupRecord.GetString(1);
            _eMissionConfig = setupRecord.GetBoolean(2);
        }
    }
}