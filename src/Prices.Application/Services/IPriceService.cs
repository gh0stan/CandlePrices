using Prices.Contracts.Dto;

namespace Prices.Application.Services
{
    public interface IPriceService
    {
        Task<double> GetAggregatedPriceAsync(string instrument, long unixTimePoint, CancellationToken token = default);
        Task<List<PricePointDto>> GetPricesInRangeAsync(string instrument, long startTime, long endTime, CancellationToken token = default);
    }
}
