// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Helpers.Exceptions;
using Econolite.Ode.Models.PavementCondition.Dto;
using Econolite.Ode.Repository.PavementCondition;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Services.PavementCondition
{
    public class PavementConditionConfigService : IPavementConditionConfigService
    {
        private readonly IPavementConditionConfigRepository _pcConfigRepository;
        private readonly ILogger<PavementConditionConfigService> _logger;

        public PavementConditionConfigService(IPavementConditionConfigRepository pcConfigRepository, ILogger<PavementConditionConfigService> logger)
        {
            _pcConfigRepository = pcConfigRepository;
            _logger = logger;
        }

        public async Task<PavementConditionConfigDto?> GetFirstAsync()
        {
            var list = await _pcConfigRepository.GetAllAsync();
            return list.Select(e => e.AdaptToDto()).FirstOrDefault();
        }

        public async Task<PavementConditionConfigDto> Add(PavementConditionConfigAdd add)
        {
            var pcs = add.AdaptToPavementConditionConfig();
            pcs.Id = Guid.NewGuid();

            //enforce that  only one record can exist
            var list = await _pcConfigRepository.GetAllAsync();
            if (list.Any()) {
                throw new AddException("Pavement condition configuration already exists");
            }

            _pcConfigRepository.Add(pcs);

            var (success, _) = await _pcConfigRepository.DbContext.SaveChangesAsync();

            return pcs.AdaptToDto();
        }

        public async Task<PavementConditionConfigDto?> Update(PavementConditionConfigUpdate update)
        {
            try
            {
                var pcs = await _pcConfigRepository.GetByIdAsync(update.Id);
                var updated = update.AdaptTo(pcs);

                _pcConfigRepository.Update(updated);

                var (success, errors) = await _pcConfigRepository.DbContext.SaveChangesAsync();
                if (!success && !string.IsNullOrWhiteSpace(errors)) throw new UpdateException(errors);
                return updated.AdaptToDto();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                _pcConfigRepository.Remove(id);
                var (success, errors) = await _pcConfigRepository.DbContext.SaveChangesAsync();
                return success;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
    }
}
