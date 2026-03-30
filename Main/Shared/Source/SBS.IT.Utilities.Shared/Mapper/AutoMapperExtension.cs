using AutoMapper;
using System;
using System.Linq;

namespace SBS.IT.Utilities.Shared.Mapper
{
    public static class AutoMapperExtension
    {
        private static IMapper _mapper;

        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                    throw new InvalidOperationException("AutoMapper has not been configured. Call Configure() first.");
                return _mapper;
            }
        }

        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                var assemblies = (from _assembly in AppDomain.CurrentDomain.GetAssemblies()
                                  where _assembly.FullName.StartsWith("SBS", StringComparison.InvariantCultureIgnoreCase)
                                  select _assembly).ToList();
                foreach (var assembly in assemblies)
                {
                    var profiles = assembly.GetTypes()
                        .Where(t => t != typeof(Profile) && typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract)
                        .ToArray();
                    foreach (var profile in profiles)
                    {
                        cfg.AddProfile((Profile)Activator.CreateInstance(profile));
                    }
                }
            });

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();

            // Keep static Mapper in sync for any legacy callers
            AutoMapper.Mapper.Initialize(x =>
            {
                var assemblies = (from _assembly in AppDomain.CurrentDomain.GetAssemblies()
                                  where _assembly.FullName.StartsWith("SBS", StringComparison.InvariantCultureIgnoreCase)
                                  select _assembly).ToList();
                foreach (var assembly in assemblies)
                {
                    var profiles = assembly.GetTypes()
                        .Where(t => t != typeof(Profile) && typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract)
                        .ToArray();
                    foreach (var profile in profiles)
                    {
                        x.AddProfile((Profile)Activator.CreateInstance(profile));
                    }
                }
            });
        }
    }
}
