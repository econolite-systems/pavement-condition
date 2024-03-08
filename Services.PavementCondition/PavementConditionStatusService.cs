// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Messaging;
using Econolite.Ode.Repository.PavementCondition;
using Econolite.Ode.Models.PavementCondition.Status;
using Econolite.Ode.Status.Common;
using Status.PavementCondition;

namespace Econolite.Ode.Services.PavementCondition
{
    public class PavementConditionStatusService : IPavementConditionStatusService
    {
        private readonly IPavementConditionStatusRepository _pcStatusRepository;
        private readonly ISink<ActionEventStatus> _actionEventStatusSink;

        public PavementConditionStatusService(IPavementConditionStatusRepository pcStatusRepository, ISink<ActionEventStatus> actionEventStatusSink)
        {
            _pcStatusRepository = pcStatusRepository;
            _actionEventStatusSink = actionEventStatusSink;
        }

        public async Task<IEnumerable<PavementConditionStatusDto>> FindAsync(bool? active)
        {
            return await _pcStatusRepository.FindAsync(active);
        }

        public async Task<IEnumerable<PavementConditionStatusDto>> FindAsync(DateTime startTime, DateTime? endTime)
        {
            return await _pcStatusRepository.FindAsync(startTime, endTime);
        }

        public async Task InsertOneAsync(PavementConditionStatusDto pcStatusDto)
        {
            await _pcStatusRepository.InsertOneAsync(pcStatusDto);
            await SendActiveStatusAsync(pcStatusDto);
        }

        public async Task InsertManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos)
        {
            await _pcStatusRepository.InsertManyAsync(pcStatusDtos);
            await SendActiveStatusAsync(pcStatusDtos);
        }

        public async Task UpdateManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos)
        {
            await _pcStatusRepository.UpdateManyAsync(pcStatusDtos);
            await SendActiveStatusAsync(pcStatusDtos);
        }

        public async Task DeleteManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos)
        {
            await _pcStatusRepository.DeleteManyAsync(pcStatusDtos);
        }

        private async Task SendActiveStatusAsync(IEnumerable<PavementConditionStatusDto> pcStatusDto)
        {
            foreach (var status in pcStatusDto.Where(s => s.IsActive).ToArray())
            {
                await SendActiveStatusAsync(status);
            }
        }

        private async Task SendActiveStatusAsync(PavementConditionStatusDto pcStatusDto)
        {
            if (!pcStatusDto.IsActive) return;
            
            var actionEventStatus = new PavementConditionStatus
            {
                ActionEventType = "PavementConditionStatus",
                Active = pcStatusDto.IsActive,
                StatusId = pcStatusDto.Id,
                TimeStamp = pcStatusDto.Timestamp,
                Location = pcStatusDto.Location,
                Latitude = pcStatusDto.Latitude,
                Longitude = pcStatusDto.Longitude,
                Severity = Enum.GetName(pcStatusDto.Severity),
                Type = Enum.GetName(pcStatusDto.Type)
            };
            await _actionEventStatusSink.SinkAsync(Guid.Empty, actionEventStatus, CancellationToken.None);
        }
    }
}
