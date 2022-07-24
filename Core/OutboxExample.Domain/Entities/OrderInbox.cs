using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.Domain.Entities
{
    public class OrderInbox
    {
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Processed { get; set; }
        public Guid IdempotentToken { get; set; }
    }
}
