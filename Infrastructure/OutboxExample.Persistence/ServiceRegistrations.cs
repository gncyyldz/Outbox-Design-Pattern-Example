using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OutboxExample.Application.Repositories;
using OutboxExample.Persistence.Context;
using OutboxExample.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.Persistence
{
    public static class ServiceRegistrations
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<OutboxExampleDbContext>(options => options.UseSqlServer("Server=localhost, 1433;Database=OutboxExampleDB;User ID=SA;Password=1q2w3e4r+!;"));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderOutboxRepository, OrderOutboxRepository>();
            services.AddScoped<IOrderInboxRepository, OrderInboxRepository>();
        }
    }
}
