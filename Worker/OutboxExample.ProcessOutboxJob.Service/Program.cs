using OutboxExample.ProcessOutboxJob.Service;
using OutboxExample.ProcessOutboxJob.Service.Jobs;
using Quartz;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddQuartz(configurator =>
        {
            configurator.UseMicrosoftDependencyInjectionJobFactory();

            JobKey jobKey = new("OrderOutboxPublishJob");

            //Bir job tanýmlanýp 'OrderOutboxPublishJob' isimli sýnýfa baðlanýyor.
            configurator.AddJob<OrderOutboxPublishJob>(options => options.WithIdentity(jobKey));

            TriggerKey triggerKey = new("OrderOutboxPublishTrigger");
            //Job 5 saniyelik aralýklarla çalýþacak þekilde ayarlanýyor.
            configurator.AddTrigger(options => options.ForJob(jobKey)
                        .WithIdentity(triggerKey)
                        .StartAt(DateTime.UtcNow)//Trigger'ýn baþlangýç tarihini belirliyoruz.
                        .WithSimpleSchedule//Trigger'ýn baþladýktan sonraki programýný belirtiyoruz.
                        (
                            builder => builder.WithIntervalInSeconds(5) //Trigger'ýn kaç saniyede bir tetikleneceðini belirliyoruz.
                                              .RepeatForever() //Trigger'ýn sonsuza denk çalýþacaðýný belirtiyoruz.
                        ));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, _configurator) =>
            {
                _configurator.Host(hostContext.Configuration["RabbitMQ:Host"], "/", hostConfigurator =>
                {
                    hostConfigurator.Username(hostContext.Configuration["RabbitMQ:Username"]);
                    hostConfigurator.Password(hostContext.Configuration["RabbitMQ:Password"]);
                });
            });
        });
    })
    .Build();

await host.RunAsync();