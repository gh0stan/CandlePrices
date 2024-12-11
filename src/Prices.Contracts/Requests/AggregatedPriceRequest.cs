using Microsoft.AspNetCore.Mvc;

namespace Prices.Contracts.Requests
{
    public class AggregatedPriceRequest
    {
        [FromRoute] 
        public string Instrument { get; set; }

        [FromQuery]
        public long TimePoint { get; set; }
    }
}
