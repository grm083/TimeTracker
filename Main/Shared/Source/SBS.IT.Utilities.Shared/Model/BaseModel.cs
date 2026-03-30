using System;

namespace SBS.IT.Utilities.Shared.Model
{
    [Serializable]
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
    [Serializable]
    public abstract class BaseLookupModel : BaseModel
    {
        public int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public string Notes { get; set; }
        public Nullable<int> SortOrder { get; set; }
    }
}
