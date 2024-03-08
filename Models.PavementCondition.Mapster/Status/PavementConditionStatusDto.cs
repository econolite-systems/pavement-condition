// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Config;

namespace Econolite.Ode.Models.PavementCondition.Status
{
    public class PavementConditionStatusDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public PavementConditionStatusSeverity Severity { get; set; }
        public PavementConditionStatusType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
