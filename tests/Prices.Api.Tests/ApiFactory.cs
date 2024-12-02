using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prices.Application.Database;
using Prices.Application.Factories;
using Prices.Application.Repositories;
using Prices.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Api.Tests
{
    public class ApiFactory : WebApplicationFactory<IApiAssemblyMarker>
    {
        private readonly Action<IServiceCollection> _configure;

        public ApiFactory(Action<IServiceCollection> configure)
        {
            _configure = configure;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureTestServices(
                services =>
                {
                    services.RemoveAll(typeof(IDbConnectionFactory));
                    services.RemoveAll(typeof(IApiCallerFactory));
                    services.RemoveAll(typeof(IPriceRepository)); 
                    services.RemoveAll(typeof(DbInitializer));

                    _configure(services);

                }
            );
        }
    }
}
