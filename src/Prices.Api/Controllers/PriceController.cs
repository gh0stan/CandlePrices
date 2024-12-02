using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System;
using Prices.Application.Services;
using Prices.Application.Validators;
using Prices.Contracts.Requests;
using Prices.Contracts.Responses;

namespace Prices.Api.Controllers
{
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _priceService;
        private readonly IOutputCacheStore _outputCacheStore;

        public PriceController(IPriceService priceService, IOutputCacheStore outputCacheStore)
        {
            _priceService = priceService;
            _outputCacheStore = outputCacheStore;
        }

        [HttpGet(ApiEndpoints.Prices.Aggregated)]
        [OutputCache(PolicyName = "Cache")]
        public async Task<IActionResult> GetAggregatedPrice([FromRoute] AggregatedPriceRequest request,
            CancellationToken token)
        {
            var validator = new PriceRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var price = await _priceService.GetAggregatedPriceAsync(request.Instrument, request.TimePoint, token);
            
            return Ok(new PriceResponse
            {
                Instrument = request.Instrument,
                TimePoint = request.TimePoint,
                AggregatedPrice = price
            });
        }

        [HttpGet(ApiEndpoints.Prices.Historical)]
        [OutputCache(PolicyName = "Cache")]
        public async Task<IActionResult> GetHistoricalPrice([FromRoute] HistoricalPricesRequest request, 
            CancellationToken token)
        {
            var validator = new HistoricalRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var prices = await _priceService.GetPricesInRangeAsync(request.Instrument, request.startTime, request.endTime, token);
            return Ok(new HistoricalPriceResponse
            {
                Instrument = request.Instrument,
                Prices = prices
            });
        }
    }
}
