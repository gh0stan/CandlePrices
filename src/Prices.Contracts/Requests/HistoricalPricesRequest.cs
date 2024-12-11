using Microsoft.AspNetCore.Mvc;

namespace Prices.Contracts.Requests
{
    public record HistoricalPricesRequest
    {
        [FromRoute] 
        public string Instrument { get; set; }
        
        [FromQuery] 
        public long startTime { get; set; }

        [FromQuery]
        public long endTime {get; set; }
    };
}
