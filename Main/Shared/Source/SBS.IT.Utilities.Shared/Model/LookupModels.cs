namespace SBS.IT.Utilities.Shared.Model
{
    public class ProjectTypeModel
    {
        public int ProjectTypeId { get; set; }
        public string ProjectTypeCode { get; set; }
        public string ProjectTypeName { get; set; }
        public string Description { get; set; }
    }

    public class ProjectItemStatusModel
    {
        public int ProjectItemStatusId { get; set; }
        public string ProjectItemStatusCode { get; set; }
        public string ProjectItemStatusName { get; set; }
        public string ProjectItemStatusDescription { get; set; }
    }

    public class WorkTypeCategoryModel : BaseModel
    {
        public int WorkTypeCategoryId { get; set; }
        public string WorkTypeCategoryCode { get; set; }
        public string WorkTypeCategoryName { get; set; }
        public string WorkTypeCategoryDescription { get; set; }
    }

}
