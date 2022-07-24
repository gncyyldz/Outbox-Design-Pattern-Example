using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutboxExample.Domain.Entities;

namespace OutboxExample.Persistence.EntityConfigurations
{
    public class OrderInboxConfiguration : IEntityTypeConfiguration<OrderInbox>
    {
        public void Configure(EntityTypeBuilder<OrderInbox> builder)
        {
            builder.HasKey(p => p.IdempotentToken);
        }
    }
}
