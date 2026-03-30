using FluentValidation;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.Http;
using Unity.WebApi;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.EntityFramework.Implementation;
using SBS.IT.Utilities.Shared.APIExtension.AppStart;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<ITrackerDbRepository, EFTimeTrackerDbRepository>(new ContainerControlledLifetimeManager());
            ConfigureFluentValidators(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        static void ConfigureFluentValidators(IUnityContainer container)
        {
            container.RegisterType<IValidatorFactory, UnityValidatorFactory>(new ContainerControlledLifetimeManager());
            var assemblies = (from _assembly in AppDomain.CurrentDomain.GetAssemblies() where _assembly.FullName.StartsWith("SBS", StringComparison.InvariantCultureIgnoreCase) select _assembly).ToList();
            foreach (var assembly in assemblies)
            {
                var validators = AssemblyScanner.FindValidatorsInAssembly(assembly);
                validators.ForEach(validator => container.RegisterType(validator.InterfaceType, validator.ValidatorType));
            }
        }
    }
}