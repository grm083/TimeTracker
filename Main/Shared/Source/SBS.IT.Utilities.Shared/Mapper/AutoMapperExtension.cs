using AutoMapper;
using System;
using System.Linq;

namespace SBS.IT.Utilities.Shared.Mapper
{
    public static class AutoMapperExtension
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            Type sourceType = typeof(TSource);
            Type destinationType = typeof(TDestination);
            TypeMap existingMaps = AutoMapper.Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (string property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(x => GetConfiguration(AutoMapper.Mapper.Configuration));
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
        private static void GetConfiguration(IConfiguration configuration)
        {
            var assemblies = (from _assembly in AppDomain.CurrentDomain.GetAssemblies() where _assembly.FullName.StartsWith("SBS", StringComparison.InvariantCultureIgnoreCase) select _assembly).ToList();
            foreach (var assembly in assemblies)
            {
                var profiles = assembly.GetTypes().Where(t => t != typeof(Profile) && typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract).ToArray();
                foreach (var profile in profiles)
                {
                    configuration.AddProfile((Profile)Activator.CreateInstance(profile));
                }
            }
        }
    }
}
