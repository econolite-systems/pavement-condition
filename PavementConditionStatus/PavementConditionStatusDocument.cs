// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Config;
using MongoDB.Bson.Serialization.Attributes;

namespace Econolite.Ode.Models.PavementCondition.Db
{
    public class PavementConditionStatusDocument
    {
        public Guid Id { get; set; }
        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }
        [BsonElement("Location")]
        public string Location { get; set; } = string.Empty;
        [BsonElement("Latitude")]
        public double Latitude { get; set; }
        [BsonElement("Longitude")]
        public double Longitude { get; set; }
        [BsonElement("Severity")]
        public PavementConditionStatusSeverity Severity { get; set; }
        [BsonElement("Type")]
        public PavementConditionStatusType Type { get; set; }
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
    }
}
