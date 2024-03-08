// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Status;

namespace Econolite.Ode.Services.PavementCondition
{
    public interface IPavementConditionStatusService
    {
        Task<IEnumerable<PavementConditionStatusDto>> FindAsync(bool? active);
        Task<IEnumerable<PavementConditionStatusDto>> FindAsync(DateTime startTime, DateTime? endTime);
        Task InsertOneAsync(PavementConditionStatusDto pcStatusDto);
        Task InsertManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos);
        Task UpdateManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos);
        Task DeleteManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos);
    }
}
