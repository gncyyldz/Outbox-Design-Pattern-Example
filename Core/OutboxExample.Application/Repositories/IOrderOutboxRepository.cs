using OutboxExample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.Application.Repositories
{
    public interface IOrderOutboxRepository : IRepository<OrderOutbox>
    {
    }
}
