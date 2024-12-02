using Prices.Application.Models;

namespace Prices.Application.Repositories
{
    public interface IPriceRepository
    {
        Task<double?> GetPriceAsync(string provider, string instrument, long timePoint, CancellationToken token = default);
        Task<bool> CreatePriceAsync(Price price, CancellationToken token = default);
    }
}
