// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Common.Extensions;
using Econolite.Ode.Messaging;
using Econolite.Ode.Messaging.Elements;
using Econolite.Ode.Messaging.Extensions;
using Econolite.Ode.Models.Status.Db;
using Econolite.Ode.Monitoring.Events.Extensions;
using Econolite.Ode.Monitoring.Metrics.Extensions;
using Econolite.Ode.Simulator.PavementCondition.Logging.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builderContext, services) =>
    {
        services.AddMessaging();
        services.Configure<MessageFactoryOptions<Guid, PavementConditionStatusMessageDocument>>(_ => { });
        services.AddSingleton<IMessageFactory<Guid, PavementConditionStatusMessageDocument>, MessageFactory<PavementConditionStatusMessageDocument>>();
        services.AddTransient<IProducer<Guid, PavementConditionStatusMessageDocument>, Producer<Guid, PavementConditionStatusMessageDocument>>();
        services.AddMetrics(builderContext.Configuration, "Pavement Condition Simulator")
            .AddUserEventSupport(builderContext.Configuration, _ =>
            {
                _.DefaultSource = "Pavement Condition Simulator";
                _.DefaultLogName = Econolite.Ode.Monitoring.Events.LogName.SystemEvent;
                _.DefaultCategory = Econolite.Ode.Monitoring.Events.Category.Server;
                _.DefaultTenantId = Guid.Empty;
            });

        services.AddHostedService<PavementConditionProducer>();
    })
    .Build();

host.LogStartup();

await host.RunAsync();
