using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Contracts.Requests
{
    public class HistoricalPricesRequest
    {

        [FromRoute]
        public string Instrument { get; set; }

        [FromQuery]
        public long startTime { get; set; }

        [FromQuery]
        public long endTime { get; set; }
    }
}
