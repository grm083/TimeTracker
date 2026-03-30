using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SBS.IT.Utilities.Shared.BaseMessage
{
    [DataContract(Name = "BaseServiceResponse", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public abstract class BaseServiceResponse
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual string LogonName { get; set; }
        [DataMember]
        public virtual string CorrelationId { get; set; }
        [DataMember]
        public virtual DateTime ResponseDateTime { get; set; }
        [DataMember]
        public virtual string ResponseIPAddress { get; set; }
        [DataMember]
        public virtual string Version { get; set; }
        [DataMember]
        public Nullable<int> RowsAffected { get; set; }
        [DataMember]
        public virtual eServiceAcknowledgeType AcknowledgeType { get; set; }
        [DataMember]
        public virtual List<ServiceError> Errors { get; set; }
    }
    [DataContract(Name = "ServiceError", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public class ServiceError
    {
        [DataMember]
        public string ErrorNumber { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string ErrorSource { get; set; }
        [DataMember]
        public string PropertyName { get; set; }
        [DataMember]
        public eServiceErrorType ErrorType { get; set; }
        public ServiceError()
        {
            this.ErrorType = eServiceErrorType.NoError;
        }
    }
    [DataContract(Name = "eServiceErrorType", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public enum eServiceErrorType
    {
        [EnumMember]
        NoError = 1,
        [EnumMember]
        Warning = 2,
        [EnumMember]
        Validation = 3,
        [EnumMember]
        Fatal = 4
    }
    [DataContract(Name = "eServiceAcknowledgeType", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public enum eServiceAcknowledgeType
    {
        [EnumMember]
        Failure = 0,
        [EnumMember]
        Success = 1
    }
    [DataContract(Name = "BaseServiceLookUp", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public abstract class BaseServiceLookup
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public virtual string Code { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public Nullable<int> SortOrder { get; set; }
    }
    [DataContract(Name = "APIResponseBase", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public abstract class APIResponseBase
    {
        [DataMember]
        public virtual string LogonName { get; set; }
        [DataMember]
        public virtual string CorrelationId { get; set; }
        [DataMember]
        public virtual string ResponseServerName { get; set; }
        [DataMember]
        public virtual string Version { get; set; }
        [DataMember]
        public Nullable<int> RowsAffected { get; set; }
        [DataMember]
        public virtual eServiceAcknowledgeType AcknowledgeType { get; set; }
        [DataMember]
        public virtual List<ServiceError> Errors { get; set; }
        [DataMember]
        public virtual bool IsSuccessful
        {
            get
            {
                if (Errors != null)
                {
                    foreach (ServiceError error in Errors)
                    {
                        if (error.ErrorType == eServiceErrorType.Validation || error.ErrorType == eServiceErrorType.Fatal)
                            return false;
                    }
                }
                return true;
            }
        }
        [DataMember]
        public virtual bool IsWarning
        {
            get
            {
                if (Errors != null)
                {
                    foreach (ServiceError error in Errors)
                    {
                        if (error.ErrorType == eServiceErrorType.Warning)
                            return true;
                    }
                }
                return false;
            }
        }
    }
}
