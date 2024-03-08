// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using MongoDB.Bson.Serialization.Attributes;

namespace Econolite.Ode.Models.PavementCondition.Db
{
    public class PavementConditionStatusAddAuditDocument
    {
        [BsonElement("Filename")]
        public string Filename { get; set; }
        [BsonElement("PavementConditionStatuses")]
        public IEnumerable<PavementConditionStatusDocument> PavementConditionStatuses { get; set; }
    }

    public class PavementConditionStatusUpdateAuditDocument
    {
        [BsonElement("PavementConditionStatuses")]
        public IEnumerable<PavementConditionStatusDocument> PavementConditionStatuses { get; set; }
    }

    public class PavementConditionStatusDeleteAuditDocument
    {
        [BsonElement("PavementConditionStatuses")]
        public IEnumerable<PavementConditionStatusDocument> PavementConditionStatuses { get; set; }
    }
}
