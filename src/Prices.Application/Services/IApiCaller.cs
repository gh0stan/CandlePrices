namespace Prices.Application.Services
{
    public interface IApiCaller
    {
        Task<double> GetClosePriceAsync(string instrument, long timePoint, CancellationToken cancellationToken);
    }
}
