// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Dto;

namespace Econolite.Ode.Services.PavementCondition
{
    public interface IPavementConditionConfigService
    {
        Task<PavementConditionConfigDto?> GetFirstAsync();
        Task<PavementConditionConfigDto> Add(PavementConditionConfigAdd add);
        Task<PavementConditionConfigDto?> Update(PavementConditionConfigUpdate update);
        Task<bool> Delete(Guid id);
    }
}
