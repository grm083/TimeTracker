using SBS.IT.Utilities.Shared.BaseMessage;
using System;

namespace SBS.IT.Utilities.Shared.APIClient.Core
{
    public interface IAPIExtension
    {
        TResponse InvokeServiceWithBasicAuth<TResponse>(Uri ServiceURL, string ServiceMethod, APIRequestBase Request);
        TResponse InvokeGet<TResponse>(Uri ServiceURL);
        TResponse InvokePost<TResponse>(Uri ServiceURL, string postData);
    }
}
