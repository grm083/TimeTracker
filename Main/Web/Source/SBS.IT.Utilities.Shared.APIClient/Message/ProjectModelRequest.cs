using SBS.IT.Utilities.Shared.BaseMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.IT.Utilities.Shared.APIClient.Message
{
    public class ProjectModelRequest : APIRequestBase
    {
        public int IsActive { get; set; }
    }

    public class AuthenticationRequest : APIRequestBase
    {
        //public string logonName { get; set; }
        //public string logonPassword { get; set; }
    }
}
