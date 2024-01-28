using System;
using System.Net.Http;
using System.Text;
using eMission.Cache;
using eMission.Model;
using Newtonsoft.Json;

namespace eMission.HTTPClient
{
    /// <summary>
    ///   HTTP Client to handle all Climatiq calls
    /// </summary>
    public class ClimatiqHTTPClient
    {
        /// <summary>
        ///   HTTPClient instance
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        ///   Constructor
        /// </summary>
        public ClimatiqHTTPClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(60);
            var climatiqSettings = ClimatiqSettings.GetSettings();
            _httpClient.BaseAddress = new Uri(climatiqSettings.Url);

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {climatiqSettings.APIKey}");
        }

        /// <summary>
        ///   Calculate freight CO2 cost
        /// </summary>
        public ClimatiqHTTPResult CalculateFreightCost(ClimatiqRequest requestModel)
        {
            string errorMessage = null;

            try
            {
                var content = new StringContent(JsonConvert
                    .SerializeObject(requestModel, 
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), 
                    Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(_httpClient.BaseAddress, content).Result;

                var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    return new ClimatiqHTTPResult
                    {
                        QueryResult = JsonConvert.DeserializeObject<ClimatiqResult>(responseContent)
                    };
                }

                try
                {
                    errorMessage = JsonConvert.DeserializeObject<ClimatiqError>(responseContent).message;
                }
                catch
                {
                    // Don't need to do anything, we just tried to convert the error to a human readable format. 
                }
            }
            catch (Exception e)
            {
                while (e?.InnerException != null) e = e.InnerException;
                errorMessage = e?.Message;
            }

            return new ClimatiqHTTPResult
            {
                Error = errorMessage
            };
        }
    }
}