using FluentAssertions;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prices.Api.Tests.TestDoubles;
using Prices.Application.Services;
using Prices.Contracts.Requests;
using System.Net;

namespace Prices.Api.Tests
{
    public class PricesEndpointSpecification
    {
        private const string RequestUri = ApiEndpoints.Prices.Aggregated;

        [Fact]
        public async Task Should_return_500_when_causes_exception()
        {
            using var client =
                CreateApiWithPriceService<StubExceptionPriceService>()
                    .CreateClient();

            var response = await client.GetAsync(
                CreateGetAggregatedPriceRequest(new AggregatedPriceRequest
                    {
                        Instrument = "btcusd",
                        TimePoint = 1672531200
                    }),
                    CancellationToken.None);

            response.Should().HaveStatusCode(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Should_return_400_when_request_validation_return_false()
        {
            using var client =
                 CreateApiWithPriceService<StubExceptionPriceService>()
                     .CreateClient();

            var response = await client.GetAsync(
                CreateGetAggregatedPriceRequest(
                    new AggregatedPriceRequest{Instrument = "incorrectInstrument", TimePoint = 1672531200}),
                CancellationToken.None);

            response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }

        private static string CreateGetAggregatedPriceRequest(AggregatedPriceRequest request)
        {
            var requestStr = ApiEndpoints.Prices.Aggregated.Replace(
                "{Instrument}", request.Instrument) + "?TimePoint=" + request.TimePoint;
            return requestStr;
        }

        private static ApiFactory CreateApiWithPriceService<T>()
        where T : class, IPriceService
        {
            var api = new ApiFactory(services =>
            {
                services.RemoveAll(typeof(IPriceService));
                services.RemoveAll(typeof(IOutputCacheStore));

                services.TryAddScoped<IPriceService, T>();
                services.TryAddSingleton<IOutputCacheStore, DummyOutputCacheStore>();
            });
            return api;
        }
    }

    
}
