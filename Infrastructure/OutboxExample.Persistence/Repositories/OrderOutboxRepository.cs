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
    public class OrderOutboxRepository : Repository<OrderOutbox>, IOrderOutboxRepository
    {
        public OrderOutboxRepository(OutboxExampleDbContext context) : base(context)
        {
        }
    }
}
