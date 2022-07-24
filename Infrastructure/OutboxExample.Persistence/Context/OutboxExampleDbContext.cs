using Microsoft.EntityFrameworkCore;
using OutboxExample.Domain.Entities;
using OutboxExample.Persistence.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.Persistence.Context
{
    public class OutboxExampleDbContext : DbContext
    {
        public OutboxExampleDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderOutbox> OrderOutboxes { get; set; }
        public DbSet<OrderInbox> OrderInboxes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderOutboxConfiguration());
            modelBuilder.ApplyConfiguration(new OrderInboxConfiguration());
        }
    }
}
