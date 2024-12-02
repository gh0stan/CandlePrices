using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prices.Contracts.Dto;
using Prices.Contracts.Responses;

namespace Prices.Application.Services
{
    public interface IPriceService
    {
        Task<double> GetAggregatedPriceAsync(string instrument, long unixTimePoint, CancellationToken token = default);
        Task<List<PricePointDto>> GetPricesInRangeAsync(string instrument, long startTime, long endTime, CancellationToken token = default);
    }
}
