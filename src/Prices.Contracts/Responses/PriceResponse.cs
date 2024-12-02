using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Contracts.Responses
{
    public class PriceResponse
    {
        public string Instrument { get; set; } 
        public long TimePoint { get; set; }
        public double AggregatedPrice { get; set; }
    }
}
