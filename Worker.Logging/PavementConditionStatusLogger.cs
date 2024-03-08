// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using System.Diagnostics;
using Econolite.Ode.Messaging;
using Microsoft.Extensions.Hosting;
using Econolite.Ode.Repository.PavementCondition;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Econolite.Ode.Models.PavementCondition.Messaging;
using Econolite.Ode.Models.Status.Db;
using Econolite.Ode.Models.PavementCondition.Status;
using Econolite.Ode.Monitoring.Metrics;
using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Monitoring.Events.Extensions;

namespace Econolite.Ode.Worker.PavementCondition;

public class PavementConditionStatusLogger : BackgroundService
{
    private readonly IConsumer<Guid, PavementConditionStatusMessage> _consumer;
    private readonly IPavementConditionStatusRepository _pcStatusLogStorage;
    private readonly ILogger<PavementConditionStatusLogger> _logger;
    private readonly IMetricsCounter _loopCounter;
    private readonly UserEventFactory _userEventFactory;

    public PavementConditionStatusLogger(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        UserEventFactory userEventFactory,
        IMetricsFactory metricsFactory,
        ILogger<PavementConditionStatusLogger> logger,
        IConsumer<Guid, PavementConditionStatusMessage> consumer
    )
    {
        _consumer = consumer;
        _logger = logger;
        _userEventFactory = userEventFactory;

        var topic = configuration[Consts.TOPICS_PCSTATUS] ?? throw new NullReferenceException($"{Consts.TOPICS_PCSTATUS} missing from config.");
        _consumer.Subscribe(topic);
        _logger.LogInformation("Subscribed topic {@}", topic);

        _loopCounter = metricsFactory.GetMetricsCounter("Pavement Condition Status Logger");

        var serviceScope = serviceProvider.CreateScope();
        _pcStatusLogStorage = serviceScope.ServiceProvider.GetRequiredService<IPavementConditionStatusRepository>();
    }

    private async Task LogPavementConditionStatusAsync(PavementConditionStatusMessageDocument status)
    {
        _logger.LogDebug("Consuming pavement condition status {@}", status);
        await _pcStatusLogStorage.InsertOneAsync(status.ToDto());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _consumer.Consume(stoppingToken);
                        Debug.Assert(result is not null);

                        try
                        {
                            var task = result.Value switch
                            {
                                PavementConditionStatusMessageDocument status => LogPavementConditionStatusAsync(status),
                                _ => Task.CompletedTask
                            };

                            await task;
                            _consumer.Complete(result);

                            _loopCounter.Increment();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Unhandled exception while processing: {@MessageType}", result.Type);

                            _logger.ExposeUserEvent(_userEventFactory.BuildUserEvent(EventLevel.Error, string.Format("Error while processing Pavement Condition: {0}", result.Value)));
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Exception thrown while trying to consume PavementConditionStatusMessage");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Worker.Logging stopping");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Exception thrown while processing pavement condition status messages");
            }
        }, stoppingToken);
    }
}
