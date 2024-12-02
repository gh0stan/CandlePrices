using Dapper;
using Prices.Application.Database;
using Prices.Application.Models;

namespace Prices.Application.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PriceRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<double?> GetPriceAsync(string provider, string instrument, long timePoint, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QuerySingleOrDefaultAsync<double?>(new CommandDefinition("""
            select p.close 
            from prices p 
            where provider = @provider
            and instrument = @instrument
            and timePoint = @timePoint
            limit 1
            """, new { provider, instrument, timePoint }, cancellationToken: token));

            return result;
        }

        public async Task<bool> CreatePriceAsync(Price price, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            
            var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into prices (id, provider, instrument, timePoint, close) 
            values (@Id, @provider, @Instrument, @TimePoint, @Close)
            """, price, cancellationToken: token));

            return result > 0;
        }
    }
}
