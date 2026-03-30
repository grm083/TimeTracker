using System;
using System.Reflection;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}