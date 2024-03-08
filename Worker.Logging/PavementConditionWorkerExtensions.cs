// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Messaging;
using Econolite.Ode.Messaging.Elements;
using Econolite.Ode.Models.PavementCondition.Messaging;
using Econolite.Ode.Models.Status.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Econolite.Ode.Worker.PavementCondition
{
    public static class PavementConditionWorkerExtensions
    {
        public static IServiceCollection AddPavementConditionConsumers(this IServiceCollection services)
        {
            services.AddTransient<IMessageFactory<Guid, PavementConditionStatusMessageDocument>, MessageFactory<Guid, PavementConditionStatusMessageDocument>>();
            services.AddTransient<IPayloadSpecialist<PavementConditionStatusMessage>, JsonPayloadSpecialist<PavementConditionStatusMessage>>();
            services.AddTransient<IBuildMessagingConfig<Guid, PavementConditionStatusMessage>, BuildMessagingConfig<Guid, PavementConditionStatusMessage>>();
            services.AddTransient<IConsumeResultFactory<Guid, PavementConditionStatusMessage>, ConsumeResultFactory<PavementConditionStatusMessage>>();
            services.AddTransient<IConsumer<Guid, PavementConditionStatusMessage>, Consumer<Guid, PavementConditionStatusMessage>>();

            return services;
        }
    }
}
