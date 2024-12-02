using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Application.Services
{
    public interface IApiCaller
    {
        Task<double> GetClosePriceAsync(string instrument, long timePoint, CancellationToken cancellationToken);
    }
}
