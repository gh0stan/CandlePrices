using Prices.Application.Services;
using Prices.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Api.Tests.TestDoubles
{
    internal class StubExceptionPriceService : IPriceService
    {
        public Task<double> GetAggregatedPriceAsync(string instrument, long unixTimePoint, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<PricePointDto>> GetPricesInRangeAsync(string instrument, long startTime, long endTime, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
