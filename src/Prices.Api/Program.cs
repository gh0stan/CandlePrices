using Prices.Api.Mapping;
using Prices.Application;
using Prices.Application.Database;

namespace Prices.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.

            builder.Services.AddOutputCache(x =>
            {
                x.AddBasePolicy(c => c.Cache());
                x.AddPolicy("Cache", c =>
                    c.Cache()
                    .Expire(TimeSpan.FromMinutes(5))
                    .SetVaryByQuery(new[] { "instrument", "timePoint", "startTime", "endTime" })
                    .Tag("prices"));
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddApplication();
            builder.Services.AddDatabase(config["Database:ConnectionString"]!);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseOutputCache();
            app.UseMiddleware<ValidationMappingMiddleware>();
            app.MapControllers();

            app.Services.GetService<DbInitializer>()?.InitializeAsync();
            
            app.Run();
        }
    }
}
