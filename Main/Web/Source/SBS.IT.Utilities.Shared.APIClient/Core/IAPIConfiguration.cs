using SBS.IT.Utilities.Shared.APIClient.Implementation;
using System.Collections.Generic;

namespace SBS.IT.Utilities.Shared.APIClient.Core
{
    public interface IAPIConfiguration
    {
        string ServiceBaseAddress { get; set; }
        string APIUsername { get; set; }
        string APIPassword { get; set; }
        string ConsumerCode { get; set; }
    }
}
