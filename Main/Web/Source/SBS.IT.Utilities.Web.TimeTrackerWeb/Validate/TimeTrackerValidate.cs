using FluentValidation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Validate
{
    public class TimeTrackerValidate
    {
    }
    public class WorkItemModelValidator : AbstractValidator<ProjectItemModel>
    {
        public WorkItemModelValidator()
        {
            //RuleFor(x => x.ApplicationId).NotNull().NotEmpty().WithMessage("Application is required");
            RuleFor(x => x.ProjectItemName).NotNull().NotEmpty().WithMessage("WorkItem Name is required");
            //RuleFor(x => x.BusinessOwner).NotNull().NotEmpty().WithMessage("Business Owner is required");
        }
    }

    public class WorkTypeModelValidator : AbstractValidator<WorkTypeModel>
    {
        public WorkTypeModelValidator()
        {
            RuleFor(x => x.WorkTypeCode).NotNull().NotEmpty().WithMessage("WorkType Code is required");
            RuleFor(x => x.WorkTypeName).NotNull().NotEmpty().WithMessage("WorkType is required");
        }
    }

    public class ApplicationModelValidator : AbstractValidator<ApplicationModel>
    {
        public ApplicationModelValidator()
        {
            RuleFor(x => x.ApplicationCode).NotNull().NotEmpty().WithMessage("Application Code is required");
            RuleFor(x => x.ApplicationName).NotNull().NotEmpty().WithMessage("Application is required");
        }
    }

    public class EmployeeModelValidator : AbstractValidator<EmployeeModel>
    {
        public EmployeeModelValidator()
        {
            RuleFor(x => x.LogonPassword).NotNull().NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPassword).NotNull().NotEmpty().WithMessage("Confirm Password is required");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.LogonPassword).WithMessage("Passwords do not match");
        }
    }
}