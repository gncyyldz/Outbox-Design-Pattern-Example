using MediatR;
using OutboxExample.Application.Repositories;
using OutboxExample.Domain.Entities;
using OutboxExample.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutboxExample.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        readonly IOrderRepository _orderRepository;
        readonly IOrderOutboxRepository _orderOutboxRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IOrderOutboxRepository orderOutboxRepository)
        {
            _orderRepository = orderRepository;
            _orderOutboxRepository = orderOutboxRepository;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            Order order = new() { Description = request.Description };
            await _orderRepository.AddAsync(order);

            OrderOutbox orderOutbox = new()
            {
                OccuredOn = DateTime.UtcNow,
                ProcessedDate = null,
                Payload = JsonSerializer.Serialize(order),
                Type = nameof(OrderCreatedEvent),
                IdempotentToken = Guid.NewGuid()
            };
            await _orderOutboxRepository.AddAsync(orderOutbox);
            await _orderOutboxRepository.SaveChangesAsync();
            return new();
        }
    }
}
