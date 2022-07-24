using Confluent.Kafka;

ConsumerConfig config = new()
{
    GroupId = "MSSQLServer.dbo.OrderOutboxes",
    BootstrapServers = "localhost:29092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(config).Build();
consumer.Subscribe("MSSQLServer.dbo.OrderOutboxes");


while (true)
{
    CancellationTokenSource cancellationTokenSource = new();
    ConsumeResult<Ignore, string> result = consumer.Consume(cancellationTokenSource.Token);

    Console.WriteLine(result.Value);
    /*
     Şu aşamadan sonra result.Value'da ki veriler ayıklanıp message broker'a gönderilebilir.
     */
}