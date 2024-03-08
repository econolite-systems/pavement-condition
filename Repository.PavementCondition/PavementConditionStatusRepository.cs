// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Persistence.Mongo.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Econolite.Ode.Models.PavementCondition.Status;
using Econolite.Ode.Models.PavementCondition.Db;

namespace Econolite.Ode.Repository.PavementCondition
{
    public class PavementConditionStatusRepository : IPavementConditionStatusRepository
    {
        private readonly ILogger<PavementConditionStatusRepository> _logger;
        private readonly IMongoCollection<PavementConditionStatusDocument> _pcStatusCollection;

        public PavementConditionStatusRepository(IConfiguration configuration, ILogger<PavementConditionStatusRepository> logger, IMongoContext mongoContext)
        {
            _logger = logger;
            _pcStatusCollection = mongoContext.GetCollection<PavementConditionStatusDocument>(configuration["Collections:PavementConditionStatuses"] ?? throw new NullReferenceException("Collections:PavementConditionStatuses missing from config."));
        }

        public async Task<IEnumerable<PavementConditionStatusDto>> FindAsync(bool? active)
        {
            var result = new List<PavementConditionStatusDto>();

            try
            {
                var filter = (active != null)
                    ? Builders<PavementConditionStatusDocument>.Filter.Where(pcs => pcs.IsActive == active)
                    : Builders<PavementConditionStatusDocument>.Filter.Empty;
                var cursor = await _pcStatusCollection.FindAsync(filter);
                result.AddRange((await cursor.ToListAsync()).Select(pcs => pcs.ToDto()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to find pavement condition statuses from Mongo");
            }

            return result.OrderBy(x => x.Timestamp);
        }

        public async Task<IEnumerable<PavementConditionStatusDto>> FindAsync(DateTime startTime, DateTime? endTime)
        {
            var result = new List<PavementConditionStatusDto>();

            try
            {
                var filter = endTime != null
                    ? Builders<PavementConditionStatusDocument>.Filter.Where(pcs => pcs.Timestamp >= startTime && pcs.Timestamp <= endTime)
                    : Builders<PavementConditionStatusDocument>.Filter.Where(pcs => pcs.Timestamp >= startTime);
                var cursor = await _pcStatusCollection.FindAsync(filter);
                result.AddRange((await cursor.ToListAsync()).Select(pcs => pcs.ToDto()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to find pavement condition statuses from Mongo");
            }

            return result.OrderBy(x => x.Timestamp);
        }

        public async Task InsertOneAsync(PavementConditionStatusDto pcStatusDto)
        {
            var pcStatusDoc = pcStatusDto.ToDoc();
            await _pcStatusCollection.InsertOneAsync(pcStatusDoc);
        }

        public async Task InsertManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos)
        {
            var pcStatusDocs = pcStatusDtos.Select(pcs => pcs.ToDoc());
            await _pcStatusCollection.InsertManyAsync(pcStatusDocs);
        }

        public async Task UpdateManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos)
        {
            foreach (var pcStatusDoc in pcStatusDtos.Select(pcs => pcs.ToDoc()))
            {
                await _pcStatusCollection.FindOneAndReplaceAsync(item => item.Id == pcStatusDoc.Id, pcStatusDoc);
            }
        }

        public async Task DeleteManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos)
        {
            foreach (var pcStatusDoc in pcStatusDtos.Select(pcs => pcs.ToDoc()))
            {
                await _pcStatusCollection.FindOneAndDeleteAsync(item => item.Id == pcStatusDoc.Id);
            }
        }
    }
}
