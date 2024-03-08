// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Db;
using Econolite.Ode.Models.Status.Db;

namespace Econolite.Ode.Models.PavementCondition.Status
{
    public static class PavementConditionStatusExtensions
    {
        public static PavementConditionStatusDto ToDto(this PavementConditionStatusDocument pcStatusDoc)
        {
            return new PavementConditionStatusDto
            {
                Id = pcStatusDoc.Id,
                Timestamp = pcStatusDoc.Timestamp,
                Location = pcStatusDoc.Location,
                Latitude = pcStatusDoc.Latitude,
                Longitude = pcStatusDoc.Longitude,
                Severity = pcStatusDoc.Severity,
                Type = pcStatusDoc.Type,
                IsActive = pcStatusDoc.IsActive,
            };
        }

        public static PavementConditionStatusDocument ToDoc(this PavementConditionStatusDto pcStatusDto)
        {
            return new PavementConditionStatusDocument
            {
                Id = pcStatusDto.Id,
                Timestamp = pcStatusDto.Timestamp,
                Location = pcStatusDto.Location,
                Latitude = pcStatusDto.Latitude,
                Longitude = pcStatusDto.Longitude,
                Severity = pcStatusDto.Severity,
                Type = pcStatusDto.Type,
                IsActive = pcStatusDto.IsActive,
            };
        }

        public static PavementConditionStatusDto ToDto(this PavementConditionStatusMessageDocument pcStatusMessageDoc)
        {
            return new PavementConditionStatusDto
            {
                Id = pcStatusMessageDoc.StatusId,
                Timestamp = pcStatusMessageDoc.TimeStamp,
                Location = pcStatusMessageDoc.Location,
                Latitude = pcStatusMessageDoc.Latitude,
                Longitude = pcStatusMessageDoc.Longitude,
                Severity = pcStatusMessageDoc.Severity,
                Type = Config.PavementConditionStatusType.None,
                IsActive = false,
            };
        }
    }
}
