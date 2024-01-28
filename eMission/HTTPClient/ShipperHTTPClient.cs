using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using eMission.Cache;
using eMission.Model;
using Newtonsoft.Json;
using static PX.Objects.RQ.RQRequestLine.FK;

namespace eMission.HTTPClient
{
    /// <summary>
    ///   HTTP Client to handle all Climatiq calls
    /// </summary>
    public class ShipperHTTPClient
    {
        /// <summary>
        ///   HTTPClient instance
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        ///   Constructor
        /// </summary>
        public ShipperHTTPClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(60);
            var climatiqSettings = ClimatiqSettings.GetSettings();
            _httpClient.BaseAddress = new Uri("https://api.shipengine.com/v1/rates/estimate");


            _httpClient.DefaultRequestHeaders.Add("API-Key", "TEST_cFKStezgl7IJU/zqVXaWSfuEm07M27TL3NaHOkbY5SE");

            //_httpClient.DefaultRequestHeaders.Add("API_KEY", $"TEST_a1RiT+L/9rPON3sDWTFmSZsprWNSVBDW6vBE7dtRYgk");
        }

        /// <summary>
        ///   Calculate freight CO2 cost
        /// </summary>
        public ShipperHTTPResult GetShipperRates(ShipperRequest requestModel)
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
                    return new ShipperHTTPResult
                    {
                        QueryResult = JsonConvert.DeserializeObject<List<ShipperResult>>(responseContent)
                    };
                }

                try
                {
                    errorMessage = JsonConvert.DeserializeObject<ShipperError>(responseContent).message;
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

            return new ShipperHTTPResult
            {
                Error = errorMessage
            };
        }
    }
}