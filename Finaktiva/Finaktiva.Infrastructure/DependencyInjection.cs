using Finaktiva.Application.Contracts.IRepositories;
using Finaktiva.Application.Contracts.IUnitOfWorks;
using Finaktiva.Infrastructure.Persistences;
using Finaktiva.Infrastructure.Repositories;
using Finaktiva.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finaktiva.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConexionSqlServer")
                ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString)
            );



            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            return services;
        }
    }
}
