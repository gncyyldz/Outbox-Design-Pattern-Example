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
    public class OrderInboxRepository : Repository<OrderInbox>, IOrderInboxRepository
    {
        public OrderInboxRepository(OutboxExampleDbContext context) : base(context)
        {
        }
    }
}
