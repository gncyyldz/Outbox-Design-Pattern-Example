using OutboxExample.Application.Repositories;
using OutboxExample.Domain.Entities;
using OutboxExample.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.Persistence.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OutboxExampleDbContext context) : base(context)
        {
        }
    }
}
