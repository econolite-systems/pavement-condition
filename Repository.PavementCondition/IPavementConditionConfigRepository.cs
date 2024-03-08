// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.

using Econolite.Ode.Models.PavementCondition.Db;
using Econolite.Ode.Persistence.Common.Repository;

namespace Econolite.Ode.Repository.PavementCondition
{
    public interface IPavementConditionConfigRepository : IRepository<PavementConditionConfig, Guid>
    {
    }
}
