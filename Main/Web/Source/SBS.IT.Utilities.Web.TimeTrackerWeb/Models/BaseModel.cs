using System;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public abstract class BaseModel
    {
        public virtual int UserId { get; set; }
        public virtual string LogonName { get; set; }
        public virtual int CreateUserId { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Nullable<int> UpdateUserId { get; set; }
        public virtual Nullable<DateTime> UpdateDate { get; set; }
        public virtual Nullable<int> DeleteUserId { get; set; }
        public Nullable<DateTime> DeleteDate { get; set; }
    }
}