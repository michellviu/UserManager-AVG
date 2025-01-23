using Core.DomainService.Interfaces.Seeders;
using Infrastructure.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.HostedService
{

    public class MigratorHostedService(IServiceProvider serviceProvider, IConfiguration _configuration) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
                await dbContext.Database.MigrateAsync(stoppingToken);
            await SeedDatabaseAsync(scope);
        }


        private static async Task SeedDatabaseAsync(IServiceScope scope)
        {
            var seeders = scope.ServiceProvider.GetRequiredService<IEnumerable<ISeedData>>();

            foreach (var seeder in seeders)
            {
                await seeder.SeedDataAsync();
            }
        }
    }
}
