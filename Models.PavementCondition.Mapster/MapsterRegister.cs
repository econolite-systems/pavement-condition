// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using System.Reflection;
using Econolite.Ode.Models.PavementCondition.Db;
using Mapster;

namespace Models.PavementCondition
{
    public class MapsterRegister : ICodeGenerationRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            config.AdaptTo("[name]Dto")
                .ApplyDefaultRule();

            config.AdaptFrom("[name]Add", MapType.Map)
                .ApplyDefaultRule()
                .ForType<PavementConditionConfig>(cfg => { cfg.Ignore(pavementCondition => pavementCondition.Id); });

            config.AdaptFrom("[name]Update", MapType.MapToTarget)
                .ApplyDefaultRule();

            config.GenerateMapper("[name]Mapper")
                .ForType<PavementConditionConfig>();
        }
    }

    internal static class RegisterExtensions
    {
        public static AdaptAttributeBuilder ApplyDefaultRule(this AdaptAttributeBuilder builder)
        {
            return builder
                .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "Econolite.Ode.Models.PavementCondition.Db")
                .ExcludeTypes(type => type.IsEnum)
                .AlterType(type => type.IsEnum || Nullable.GetUnderlyingType(type)?.IsEnum == true, typeof(string))
                .ShallowCopyForSameType(true);
        }
    }
}
