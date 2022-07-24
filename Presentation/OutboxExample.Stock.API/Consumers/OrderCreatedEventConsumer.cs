using MassTransit;
using OutboxExample.Application.Repositories;
using OutboxExample.Domain.Entities;
using OutboxExample.Shared.Events;

namespace OutboxExample.Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        readonly IOrderInboxRepository _orderInboxRepository;

        public OrderCreatedEventConsumer(IOrderInboxRepository orderInboxRepository)
        {
            _orderInboxRepository = orderInboxRepository;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            bool hasData = _orderInboxRepository.GetWhere(oi => oi.IdempotentToken == context.Message.IdempotentToken && oi.Processed).Any();
            /*
             hasData : true ise demek ki bu event önceden gelmiş ve başarıyla işlenmiş lakin publisher tarafından gönderildiğine
             dair güncelleme işlemi bilinmeyen bir sebepten dolayı gerçekleştirilemediği için tekrardan
             publish edilmiştir. İşte idempotent özelliği sayesinde bu event'in tekrar işlenmesinin önüne böylece geçilmektedir.
             */

            if (!hasData)
            {
                await _orderInboxRepository.AddAsync(new()
                {
                    Description = context.Message.Description,
                    IdempotentToken = context.Message.IdempotentToken,
                    OrderId = context.Message.OrderId,
                    Quantity = context.Message.Quantity,
                    Processed = false
                });
                await _orderInboxRepository.SaveChangesAsync();
            }

            //Inbox table'a kaydedilen mesaj/event çekilip işleme tabi tutulmaktadır.

            List<OrderInbox> orderInboxes = _orderInboxRepository.GetWhere(oi => !oi.Processed).ToList();
            foreach (var orderInbox in orderInboxes)
            {
                Console.WriteLine(@$"OrderId : {orderInbox.OrderId}
                                         Id : {orderInbox.IdempotentToken}
                                         Stock {orderInbox.Quantity} miktar kadar düşürülmüştür!");
                orderInbox.Processed = true;
            }
            await _orderInboxRepository.SaveChangesAsync();

        }
    }
}
