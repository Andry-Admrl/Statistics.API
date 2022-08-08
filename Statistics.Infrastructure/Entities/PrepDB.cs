using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Statistics.Infrastructure.Entities
{
    public static class PrepDB
    {
        public static void Prepopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<StatisticsContext>());
            }
        }
        public static void SeedData(StatisticsContext context)
        {
            Console.WriteLine("Updating Database..");
            context.Database.Migrate();
        }
    }
}
