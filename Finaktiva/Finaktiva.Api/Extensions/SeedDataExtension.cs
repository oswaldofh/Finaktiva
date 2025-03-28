using Bogus;
using Finaktiva.Infrastructure.Persistences;

namespace Finaktiva.Api.Extensions
{
    public static class SeedDataExtension
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var sqlConectionFactory = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var faker = new Faker();

            List<object> vehicles = new();

            for (int i = 0; i < 100; i++)
            {
                vehicles.Add(new
                {
                    Id = Guid.NewGuid(),
                    Vin = faker.Vehicle.Vin(),
                    Model = faker.Vehicle.Model(),
                    Country = faker.Address.Country(),
                    Department = faker.Address.State(),
                    Province = faker.Address.County(),
                    City = faker.Address.City(),
                    Street = faker.Address.StreetAddress(),
                    PriceAmount = faker.Random.Decimal(1000, 20000),
                    PriceCurrencyRate = "USD",
                    MaintenanceAmount = faker.Random.Decimal(100, 200),
                    MaintenanceCurrencyRate = "USD",
                    LastRentDate = DateTime.MinValue,
                });
            }

        }
    }
}
