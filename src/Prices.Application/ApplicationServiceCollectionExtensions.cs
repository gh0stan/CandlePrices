using Microsoft.Extensions.DependencyInjection;
using Prices.Application.Factories;
using Prices.Application.Services;
using Prices.Application.Database;
using Prices.Application.Repositories;

namespace Prices.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //services.AddMemoryCache();
            services.AddHttpClient();
            services.AddTransient<IApiCallerFactory, ApiCallerFactory>();
            services.AddTransient<IPriceRepository, PriceRepository>();
            services.AddTransient<IPriceService, PriceService>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services,
            string connectionString)
        {
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(connectionString));
            services.AddSingleton<DbInitializer>();
            return services;
        }
    }
}
