// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Status;

namespace Econolite.Ode.Repository.PavementCondition
{
    public interface IPavementConditionStatusRepository
    {
        /// <summary>
        ///     Find pavement condition statuses
        /// </summary>
        /// <param name="active"></param>
        /// <returns>A list of pavement condition statuses</returns>
        Task<IEnumerable<PavementConditionStatusDto>> FindAsync(bool? active);

        /// <summary>
        ///     Find pavement condition statuses in time range
        /// </summary>
        Task<IEnumerable<PavementConditionStatusDto>> FindAsync(DateTime startTime, DateTime? endTime);

        /// <summary>
        ///     Inserts one new pavement condition status into the collection.
        /// </summary>
        /// <param name="pcStatusDto">The new pavement condition status to insert</param>
        Task InsertOneAsync(PavementConditionStatusDto pcStatusDto);

        /// <summary>
        ///     Inserts many new pavement condition statuses into the collection.
        /// </summary>
        /// <param name="pcStatusDtos">The new pavement condition statuses to insert</param>
        Task InsertManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos);

        /// <summary>
        ///     Updates many pavement condition statuses in the collection.
        /// </summary>
        /// <param name="pcStatusDtos">The pavement condition statuses to update</param>
        Task UpdateManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos);

        /// <summary>
        ///     Deletes many pavement condition statuses in the collection.
        /// </summary>
        /// <param name="pcStatusDtos">The pavement condition statuses to delete</param>
        /// <returns></returns>
        Task DeleteManyAsync(IEnumerable<PavementConditionStatusDto> pcStatusDtos);
    }
}
