using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Orders.Data.Migrations;

public static class Registrator
{
    public static IServiceCollection AddOrdersDatabase(this IServiceCollection services)
    {
        services.AddDbContext<OrdersDbContext>((p, o) =>
            o.UseSqlServer(
                p.GetRequiredService<IConfiguration>().GetConnectionString("Default"),
                b => b.MigrationsAssembly(typeof(Registrator).Assembly.FullName)
                ));

        return services;
    }
}