// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.PavementCondition.Messaging;
using Econolite.Ode.Models.PavementCondition.Config;

namespace Econolite.Ode.Models.Status.Db
{
    //TODO:  ko - this is a close guess for now; we don't have their api docs yet
    //NOTE:  letting Mongo auto generate the id since it will be timescale
    public sealed record PavementConditionStatusMessageDocument(
    Guid StatusId, //The unique id coming from the 3rd party api?
    string Location,
    DateTime TimeStamp,
    double Latitude,
    double Longitude,
    PavementConditionStatusSeverity Severity) : PavementConditionStatusMessage
    {

    }
}
