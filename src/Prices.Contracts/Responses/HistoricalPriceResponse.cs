using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prices.Contracts.Dto;

namespace Prices.Contracts.Responses
{
    public class HistoricalPriceResponse
    {
        public string Instrument { get; set; }
        public List<PricePointDto> Prices { get; set; }
    }
}
