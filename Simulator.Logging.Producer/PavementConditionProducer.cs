// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Messaging;
using Econolite.Ode.Messaging.Elements;
using Econolite.Ode.Models.PavementCondition.Config;
using Econolite.Ode.Models.Status.Db;
using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Monitoring.Events.Extensions;
using Econolite.Ode.Monitoring.Metrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Simulator.PavementCondition.Logging.Producer
{
    public class PavementConditionProducer : IHostedService
    {

        private readonly ILogger<PavementConditionProducer> _logger;
        private readonly string _statusTopic;
        private readonly IProducer<Guid, PavementConditionStatusMessageDocument> _statusProducer;
        private readonly IMessageFactory<Guid, PavementConditionStatusMessageDocument> _messageFactory;
        private readonly UserEventFactory _userEventFactory;

        private readonly IMetricsCounter _loopCounter;
        //private Guid _tenantId;

        public PavementConditionProducer(
            IMessageFactory<Guid, PavementConditionStatusMessageDocument> messageFactory,
            IProducer<Guid, PavementConditionStatusMessageDocument> producer,
            IConfiguration config,
            IMetricsFactory metricsFactory,
            UserEventFactory userEventFactory,
            ILogger<PavementConditionProducer> logger
        )
        {
            _messageFactory = messageFactory;
            _statusProducer = producer;
            _userEventFactory = userEventFactory;
            _logger = logger;

            _statusTopic = config[Consts.TOPICS_PCSTATUS] ?? throw new NullReferenceException($"{Consts.TOPICS_PCSTATUS} missing in config.");
            //_tenantId = Guid.Parse(config[Consts.TENANT_ID_HEADER]);

            _logger.LogInformation("Subscribed topic {@}", _statusTopic);

            _loopCounter = metricsFactory.GetMetricsCounter("Simulator");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("\"Producing pavement condition status message 1\"");
                
                var id = Guid.NewGuid();
                var latitude = 42.51141293090279;
                var longitude = -83.04690729506515;
                var pcStatusMessage = new PavementConditionStatusMessageDocument(
                    id,
                    "Meijer Express Gas Station",
                    DateTime.UtcNow,
                    latitude,
                    longitude,
                    PavementConditionStatusSeverity.High
                );

                await _statusProducer.ProduceAsync(_statusTopic, _messageFactory.Build(Guid.NewGuid(), pcStatusMessage));

                _logger.LogInformation("\"Producing pavement condition status message 2\"");

                id = Guid.NewGuid();
                latitude = 42.53802149624186;
                longitude = -83.04767983135882;
                var pcStatusMessage2 = new PavementConditionStatusMessageDocument(
                    id,
                    "Mound Road Crushed Concrete",
                    DateTime.UtcNow,
                    latitude,
                    longitude,
                    PavementConditionStatusSeverity.Medium
                );
                await _statusProducer.ProduceAsync(_statusTopic, _messageFactory.Build(Guid.NewGuid(), pcStatusMessage2));

                _logger.ExposeUserEvent(_userEventFactory.BuildUserEvent(EventLevel.Information, "Created pavement condition status messages."));

                _loopCounter.Increment(2);
            }
            finally
            {
                _logger.LogInformation("\"Finished producing pavement condition status messages\"");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
