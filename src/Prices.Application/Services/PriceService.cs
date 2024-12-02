using Prices.Application.Factories;
using Prices.Application.Models;
using Prices.Contracts.Dto;
using Prices.Application.Repositories;

namespace Prices.Application.Services
{
    public class PriceService : IPriceService
    {
        private readonly IPriceRepository _repository;
        private readonly IApiCallerFactory _apiCallerFactory;
        private readonly List<string> _providersFromDatabase = new List<string>
        {
            "bitfinex",
            "bitstamp"
        };

        public PriceService(IPriceRepository repository, IHttpClientFactory httpClientFactory, IApiCallerFactory apiCallerFactory, 
            CancellationToken token = default)
        {
            _repository = repository;
            _apiCallerFactory = apiCallerFactory;
        }

        public async Task<double> GetAggregatedPriceAsync(string instrument, long unixTimePoint, CancellationToken token = default)
        {
            var tasks = new List<Task<double?>>();
            foreach (var provider in _providersFromDatabase)
            {
                tasks.Add(Task.Run(async () =>
                    {
                        var storedPrice = await _repository.GetPriceAsync(provider, instrument, unixTimePoint, token);
                        if (storedPrice != null)
                        {
                            return storedPrice.Value;
                        }

                        var value = await ImportClosePriceAsync(provider, instrument, unixTimePoint, token);
                        return value;
                    }));
            }

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
            List<long> timePoints = new List<long>();
            long step = 3600;

            for (long currentPoint = startTime; currentPoint <= endTime; currentPoint += step)
            {
                timePoints.Add(currentPoint);
            }

            var tasks = new List<Task<PricePointDto>>();
            foreach (var timePoint in timePoints)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var aggregatedPrice = await GetAggregatedPriceAsync(instrument, timePoint, token);

                    return new PricePointDto
                    {
                        TimePoint = timePoint,
                        AggregatedPrice = aggregatedPrice
                    };
                }));
            }
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
                    });

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
