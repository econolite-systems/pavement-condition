// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Db;
using Econolite.Ode.Persistence.Mongo.Context;
using Econolite.Ode.Persistence.Mongo.Repository;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Repository.PavementCondition
{
    public class PavementConditionConfigRepository : GuidDocumentRepositoryBase<PavementConditionConfig>, IPavementConditionConfigRepository
    {
        public PavementConditionConfigRepository(IMongoContext context, ILogger<PavementConditionConfigRepository> logger) : base(context, logger)
        {
        }
    }
}
