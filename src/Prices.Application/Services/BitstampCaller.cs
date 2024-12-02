﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Prices.Application.Services
{
    public class BitstampCaller : IApiCaller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BitstampCaller(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        public async Task<double> GetClosePriceAsync(string instrument, long timePoint, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var endTime = timePoint + 3600;
            string apiUrl = $"https://api-pub.bitfinex.com/v2/candles/trade:1h:t{instrument.ToUpper()}/hist?start={timePoint}000&end={endTime}000&limit=1";
            var response = await httpClient.GetStringAsync(apiUrl, cancellationToken);
            JArray priceResponse = JArray.Parse(response);
            string priceStr = (string)priceResponse[0][4];

            if (double.TryParse(priceStr, out double value))
            {
                return value;
            }

            throw new Exception(string.Format("Failed to recieve proper response. {0}", response));
        }
    }
}