﻿using Newtonsoft.Json.Linq;

namespace Prices.Application.Services
{
    public class BitfinexCaller : IApiCaller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BitfinexCaller(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        public async Task<double> GetClosePriceAsync(string instrument, long timePoint, CancellationToken cancellationToken)
        {
            //retries?
            var httpClient = _httpClientFactory.CreateClient();
            string apiUrl = $"https://www.bitstamp.net/api/v2/ohlc/{instrument}/?step=3600&limit=1&start={timePoint}";
            var jsonResponse = await httpClient.GetStringAsync(apiUrl, cancellationToken);
            JObject jsonObject = JObject.Parse(jsonResponse);
            string closeValue = (string)jsonObject["data"]["ohlc"][0]["close"];
            if (double.TryParse(closeValue, out double value))
            {
                return value;
            }

            throw new Exception($"Failed to recieve proper response. {jsonResponse}");
        }
    }
}
