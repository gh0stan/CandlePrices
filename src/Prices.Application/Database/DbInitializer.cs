using Dapper;

namespace Prices.Application.Database
{
    public class DbInitializer
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DbInitializer(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync("""
            create table if not exists prices (
            id UUID primary key,
            instrument TEXT not null,
            provider TEXT not null,
            timePoint BIGINT not null,
            close DOUBLE PRECISION not null);
        """);

            await connection.ExecuteAsync("""
            create unique index concurrently if not exists prices_idx_unique
            on prices
            using btree(instrument, provider, timepoint);
        """);
        }
    }
}
