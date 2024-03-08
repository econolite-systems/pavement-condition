// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Microsoft.Extensions.DependencyInjection;
using Status.Common.Messaging.Extensions;

namespace Econolite.Ode.Services.PavementCondition
{
    public static class PavementConditionServicesExtensions
    {
        public static IServiceCollection AddPavementConditionConfigService(this IServiceCollection services)
        {
            services.AddScoped<IPavementConditionConfigService, PavementConditionConfigService>();
            return services;
        }

        public static IServiceCollection AddPavementConditionStatusService(this IServiceCollection services)
        {
            services.AddActionEventStatusSink();
            services.AddScoped<IPavementConditionStatusService, PavementConditionStatusService>();
            return services;
        }
    }
}
