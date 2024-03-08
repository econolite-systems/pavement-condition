// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Microsoft.Extensions.DependencyInjection;

namespace Econolite.Ode.Repository.PavementCondition
{
    public static  class PavementConditionRepositoryExtensions
    {
        public static IServiceCollection AddPavementConditionConfigRepository(this IServiceCollection services)
        {
            services.AddScoped<IPavementConditionConfigRepository, PavementConditionConfigRepository>();
            return services;
        }

        public static IServiceCollection AddPavementConditionStatusRepository(this IServiceCollection services)
        {
            services.AddScoped<IPavementConditionStatusRepository, PavementConditionStatusRepository>();
            return services;
        }
    }
}
