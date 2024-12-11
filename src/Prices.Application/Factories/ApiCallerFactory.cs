using Prices.Application.Services;

namespace Prices.Application.Factories
{
    public interface IApiCallerFactory
    {
        IApiCaller GetApiCaller(string provider);
    }

    public class ApiCallerFactory : IApiCallerFactory
    {
        private readonly Dictionary<string, Func<IApiCaller>> _apiCallers;

        public ApiCallerFactory(IHttpClientFactory httpClientFactory)
        {
            _apiCallers = new Dictionary<string, Func<IApiCaller>>
            {
                { "bitfinex", () => new BitfinexCaller(httpClientFactory) },
                { "bitstamp", () => new BitstampCaller(httpClientFactory) }
            };
        }

        public IApiCaller GetApiCaller(string provider)
        {
            if (!_apiCallers.TryGetValue(provider, value: out var apiCaller))
            {
                throw new ArgumentException(nameof(provider));
            }

            var caller = apiCaller.Invoke();
            return caller;
        }
    }
}
