using FluentValidation;
using FluentValidation.Mvc;
using System;
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.App_Start
{
    public class FluentValidationConfig
    {
        public static void RegisterFluentValidation()
        {
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new UnityValidatorFactory();
            });
        }
    }

    public class UnityValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return DependencyResolver.Current.GetService(validatorType) as IValidator;
        }
    }
}