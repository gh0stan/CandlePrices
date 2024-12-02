using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
