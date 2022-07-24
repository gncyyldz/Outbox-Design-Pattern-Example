using Dapper;
using MassTransit;
using OutboxExample.Domain.Entities;
using OutboxExample.ProcessOutboxJob.Service.Contexts;
using OutboxExample.Shared.Events;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutboxExample.ProcessOutboxJob.Service.Jobs
{
    public class OrderOutboxPublishJob : IJob
    {
        readonly IPublishEndpoint _publishEndpoint;

        public OrderOutboxPublishJob(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (OrderSingletonDatabase.DataReaderState)
            {
                OrderSingletonDatabase.DataReaderBusy();
                List<OrderOutbox> orderOutboxes = (await OrderSingletonDatabase.QueryAsync<OrderOutbox>($@"SELECT * FROM OrderOutboxes
                                                                                                           WHERE ProcessedDate IS NULL
                                                                                                           ORDER By OccuredOn DESC"))
                                                                        .ToList();
                foreach (OrderOutbox orderOutbox in orderOutboxes)
                {
                    if (orderOutbox.Type == nameof(OrderCreatedEvent))
                    {
                        Order? order = JsonSerializer.Deserialize<Order>(orderOutbox.Payload);
                        if (order != null)
                        {
                            OrderCreatedEvent orderCreatedEvent = new()
                            {
                                Description = order.Description,
                                OrderId = order.Id,
                                Quantity = order.Quantity,
                                IdempotentToken = orderOutbox.IdempotentToken
                            };

                            await _publishEndpoint.Publish(orderCreatedEvent);
                        }
                    }

                    /*
                        Outbox table'da ki publish edilmemiş mesajları/event'leri message
                        broker'a publish ettik. Şimdi yayınlanmış bu mesajların yayınlandığına
                        dair veritabanında Outbox table'da gerekli güncellemelerin/işaretlemelerin
                        yapılması gerekmektedir(ya da yayınlanmış mesaja dair kayıt silinmelidir)
                        İşte tam bu noktada Outbox table'ın olduğu veritabanı ile yaşanabilecek
                        olası bağlantı kopukluklarından dolayı Idempotent tasarımı kullanıyor olacağız.
                        Çünkü bir sonraki job'ın tetiklenme sürecinde bu işlendiğine dair güncellenemeyen
                        ama özünde işlenen mesajlar tekrardan/yineli bir şekilde message broker'a gönderilecek
                        ve consumer tarafından veri tutarsızlığına meydan verebilecek şekilde tüketilecektir.
                        Consumer gerektiği taktirde Inbox pattern'ın getirisi olan Inbox
                        table'ı kullanarak hangi mesajları/event'leri işleyip işlemediğini tutacak ve
                        ihtimal olarak yinelenebilecek mesajların tutarsızlığa sebebiyet verebilecek durumları
                        böylece engellenmiş olacaktır.
                     */
                    int result = await OrderSingletonDatabase.ExecuteAsync(@$"UPDATE OrderOutboxes SET ProcessedDate = GETDATE()
                                                                              WHERE IdempotentToken = '{orderOutbox.IdempotentToken}'");

                }
                OrderSingletonDatabase.DataReaderReady();
                Console.WriteLine("Order outbox table checked!");
            }

        }
    }
}
