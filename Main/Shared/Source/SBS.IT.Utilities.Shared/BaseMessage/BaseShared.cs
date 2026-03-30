using System;
using System.Collections.Generic;

namespace SBS.IT.Utilities.Shared.BaseMessage
{
    [Serializable]
    public abstract class BaseSharedRequest
    {
        public virtual int UserId { get; set; }
        public virtual string LogonName { get; set; }
        public virtual string CorrelationId { get; set; }
    }
    [Serializable]
    public abstract class BaseSharedResponse
    {
        public virtual eSharedAcknowledgeType AcknowledgeType { get; set; }
        public virtual string CorrelationId { get; set; }
        public virtual List<SharedError> Errors { get; set; }
    }
    [Serializable]
    public class SharedError
    {
        public string ErrorNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }
        public string PropertyName { get; set; }
        public eSharedErrorType ErrorType { get; set; }
        public SharedError()
        {
            this.ErrorType = eSharedErrorType.NoError;
        }
    }
    [Serializable]
    public enum eSharedErrorType
    {
        NoError = 1,
        Warning = 2,
        Validation = 3,
        Fatal = 4
    }
    [Serializable]
    public enum eSharedAcknowledgeType
    {
        Failure = 0,
        Success = 1
    }
}
