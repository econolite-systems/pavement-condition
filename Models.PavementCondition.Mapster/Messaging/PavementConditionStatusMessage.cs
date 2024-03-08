// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
namespace Econolite.Ode.Models.PavementCondition.Messaging
{
    public abstract record PavementConditionStatusMessage;

    public sealed record UnknownPavementConditionStatusMessage(string Type, string Data) : PavementConditionStatusMessage;

    public sealed record NonParseablePavementConditionStatusMessage(
        string Type,
        string Data,
        Exception Exception
    ) : PavementConditionStatusMessage;
}
