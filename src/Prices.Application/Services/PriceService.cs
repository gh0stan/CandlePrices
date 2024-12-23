﻿using Prices.Application.Factories;
using Prices.Application.Models;
using Prices.Contracts.Dto;
using Prices.Application.Repositories;

namespace Prices.Application.Services
{
    public class PriceService : IPriceService
    {
        private readonly IPriceRepository _repository;
        private readonly IApiCallerFactory _apiCallerFactory;
        private readonly List<string> _providersFromDatabase =
        [
            "bitfinex",
            "bitstamp"
        ];

        public PriceService(IPriceRepository repository, IApiCallerFactory apiCallerFactory)
        {
            _repository = repository;
            _apiCallerFactory = apiCallerFactory;
        }

        public async Task<double> GetAggregatedPriceAsync(string instrument, long unixTimePoint, CancellationToken token = default)
        {
            var tasks = _providersFromDatabase.Select(provider => Task.Run(async () =>
                {
                    var storedPrice = await _repository.GetPriceAsync(provider, instrument, unixTimePoint, token);
                    if (storedPrice != null)
                    {
                        return storedPrice.Value;
                    }

                    var value = await ImportClosePriceAsync(provider, instrument, unixTimePoint, token);
                    return value;
                }, token))
                .ToList();

            await Task.WhenAll(tasks);
            
            var closePrices = new List<double>();
            foreach(var task in tasks)
            {
                if (task.Result != null)
                {
                    closePrices.Add(task.Result.Value);
                }
            }

            if (closePrices.Count > 0)
            {
                return closePrices.Average();
            }

            throw new Exception("Unable to get data");
        }

        public async Task<List<PricePointDto>> GetPricesInRangeAsync(string instrument, long startTime, long endTime, CancellationToken token = default)
        {
            var timePoints = new List<long>();
            const long step = 3600;

            for (var currentPoint = startTime; currentPoint <= endTime; currentPoint += step)
            {
                timePoints.Add(currentPoint);
            }

            var tasks = timePoints.Select(timePoint => Task.Run(async () =>
                {
                    var aggregatedPrice = await GetAggregatedPriceAsync(instrument, timePoint, token);

                    return new PricePointDto { TimePoint = timePoint, AggregatedPrice = aggregatedPrice };
                }, token))
                .ToList();
            
            await Task.WhenAll(tasks);

            var result = new List<PricePointDto>();
            foreach (var task in tasks)
            {
                if (task.Result != null)
                {
                    result.Add(task.Result);
                }
            }

            return result;
        }

        private async Task<double?> ImportClosePriceAsync(string provider, string instrument, long unixTimePoint, CancellationToken token = default)
        {
            try
            {
                var apiCaller = _apiCallerFactory.GetApiCaller(provider);
                var value = await apiCaller.GetClosePriceAsync(instrument, unixTimePoint, token);
                
                await _repository.CreatePriceAsync(
                    new Price
                    {
                        Id = Guid.NewGuid(),
                        Provider = provider,
                        Instrument = instrument,
                        TimePoint = unixTimePoint,
                        Close = value
                    }, token);

                return value;
            }
            catch (Exception ex)
            {
                //logexception
                return null;
            }
        }
    }
}
