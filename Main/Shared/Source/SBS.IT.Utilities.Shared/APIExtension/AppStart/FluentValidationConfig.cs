using FluentValidation;
using FluentValidation.WebApi;
using System;
using System.Web.Http;
using System.Web.ModelBinding;

namespace SBS.IT.Utilities.Shared.APIExtension.AppStart
{
    public class UnityValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return GlobalConfiguration.Configuration.DependencyResolver.GetService(validatorType) as IValidator;
        }
    }
    //public class FluentValidationConfig
    //{
    //    public static void RegisterValidators()
    //    {
    //        DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
    //        ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
    //        FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration, provider =>
    //        {
    //            provider.ValidatorFactory = new UnityValidatorFactory();
    //        });
    //    }
    //}
}
