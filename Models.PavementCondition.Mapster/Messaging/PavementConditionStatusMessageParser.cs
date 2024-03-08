// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Status.Db;
using System.Text.Json;

namespace Econolite.Ode.Models.PavementCondition.Messaging
{
    public class PavementConditionStatusMessageParser
    {
        public PavementConditionStatusMessage Parse(string type, string data)
        {
            try
            {
                return type switch
                {                    
                    nameof(PavementConditionStatusMessageDocument) => JsonSerializer.Deserialize<PavementConditionStatusMessageDocument>(data)!,
                    _ => new UnknownPavementConditionStatusMessage(type, data)
                };
            }
            catch (Exception ex)
            {
                return new NonParseablePavementConditionStatusMessage(type, data, ex);
            }
        }
    }
}
