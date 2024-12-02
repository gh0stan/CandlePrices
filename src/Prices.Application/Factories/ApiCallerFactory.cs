using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiCallerFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _apiCallers = new Dictionary<string, Func<IApiCaller>>
            {
                { "bitfinex", () => new BitfinexCaller(_httpClientFactory) },
                { "bitstamp", () => new BitstampCaller(_httpClientFactory) }
            };
        }

        public IApiCaller GetApiCaller(string provider)
        {
            if (!_apiCallers.ContainsKey(provider))
            {
                throw new ArgumentException(nameof(provider));
            }

            var caller = _apiCallers[provider].Invoke();
            return caller;
        }
    }
}
