using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.IT.Utilities.Shared.Mapper;

namespace SBS.IT.Utilities.Shared.APIExtension.AppStart
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMapper()
        {
            AutoMapperExtension.Configure();
        }
    }
}
