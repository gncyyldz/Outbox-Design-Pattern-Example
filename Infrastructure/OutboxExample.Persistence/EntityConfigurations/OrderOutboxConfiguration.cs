using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxExample.Domain.Entities;

namespace OutboxExample.Persistence.EntityConfigurations
{
    public class OrderOutboxConfiguration : IEntityTypeConfiguration<OrderOutbox>
    {
        public void Configure(EntityTypeBuilder<OrderOutbox> builder)
        {
            builder.HasKey(p => p.IdempotentToken);
        }
    }
}
