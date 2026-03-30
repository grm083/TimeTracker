using AutoMapper;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Edmx;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;

namespace SBS.IT.Utilities.DataAccess.TimeTrackerDb.EntityFramework.Mapper
{
    class ModelToDomainModelMapper : Profile
    {
        public override string ProfileName
        {
            get
            {
                return GetType().Name;
            }
        }
        protected override void Configure()
        {

        }
    }
}
