using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class ProjectModel : BaseModel
    {
        public int ProjectId { get; set; }
        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "Project Name is required")]
        public string ProjectName { get; set; }
        [Display(Name = "Description")]
        public string ProjectDescription { get; set; }
        public List<ProjectTypeModel> ProjectTypelst { get; set; }
        public int? RowTotal { get; set; }

        [Required(ErrorMessage = "Project Type is required")]
        public int ProjectTypeId { get; set; }

        [Display(Name = "Project Type")]
        public string ProjectTypeName { get; set; }

        [Display(Name = "Is Active")]
        public int IsActive { get; set; }

    }

    public class ProjectListModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }



    public class ProjectView
    {
        [Required(ErrorMessage = "Requried, Please select any project!")]

        public int ProjectId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public class ProjectViewResponse
    {
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public Nullable<decimal> WorkHour { get; set; }
    }

    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            this.ProjectViews = new List<ProjectViewResponse>();
        }
        public string EmployeeTypeCode { get; set; }
        public List<ProjectViewResponse> ProjectViews { get; set; }
        public decimal? EmployeeTypeTotalHour { get; set; }
        public string ProjectName { get; set; }
        public string EmployeeType { get; set; }
        public decimal? TotalHour { get; set; }
    }

    public class ProjectViewExportResponse
    {
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public Nullable<decimal> WorkHour { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class ExportProjectView
    {
        public ExportProjectView()
        {
            workTypes = new List<WorkType>();
        }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal? TotalHour { get; set; }
        public List<WorkType> workTypes { get; set; }
    }

    public class WorkType
    {
        public WorkType()
        {
            ProjectViewExportResponses = new List<ProjectViewExportResponse>();
        }

        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public decimal? TotalHour { get; set; }
        public List<ProjectViewExportResponse> ProjectViewExportResponses { get; set; }
    }

    public class ExportProjectViewModel
    {
        public ExportProjectViewModel()
        {
            ExportProjectViews = new List<ExportProjectView>();
            EmployeeList = new List<SelectListItem>();
        }
        public List<ExportProjectView> ExportProjectViews { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
    }
}