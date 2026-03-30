using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SBS.IT.Utilities.Shared.BaseMessage
{
    [DataContract(Name = "BaseServiceRequest", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public abstract class BaseServiceRequest
    {
        [DataMember]
        public virtual int UserId { get; set; }
        [DataMember]
        public virtual string LogonName { get; set; }
        [DataMember]
        public virtual string CorrelationId { get; set; }
        [DataMember]
        public virtual DateTime RequestDateTime { get; set; }
        [DataMember]
        public virtual string ClientIPAddress { get; set; }
        [DataMember]
        public virtual string ServerIPAddress { get; set; }
        [DataMember]
        public virtual eServiceApplicationChannel ApplicationChannel { get; set; }
        [DataMember]
        public virtual ServicePaging ServicePaging { get; set; }
    }
    [DataContract(Name = "eServiceApplicationChannel", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public enum eServiceApplicationChannel
    {
        [EnumMember]
        AcornWeb = 1,
        [EnumMember]
        EBusinessClient = 2,
        [EnumMember]
        EBusinessSupplier = 3,
        [EnumMember]
        WMAcornWeb = 4,
        [EnumMember]
        CPQ = 5,
        [EnumMember]
        FinanceManagementWeb = 6,
        [EnumMember]
        STARS = 7,
        [EnumMember]
        Processor = 99,
        [EnumMember]
        Acorn = 8,
        [EnumMember]
        CustomerManagement = 9
    }
    [DataContract(Name = "ServicePaging", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public class ServicePaging
    {
        [DataMember]
        public virtual Nullable<int> PageNumber { get; set; }
        [DataMember]
        public virtual Nullable<int> PageSize { get; set; }
        [DataMember]
        public virtual List<ServiceOrderBy> ServiceOrderBy { get; set; }
    }
    [DataContract(Name = "ServiceOrderBy", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public class ServiceOrderBy
    {
        [DataMember]
        public string Column { get; set; }
        [DataMember]
        public eServiceDirection Direction { get; set; }
    }
    [DataContract(Name = "eServiceDirection", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public enum eServiceDirection
    {
        [EnumMember]
        Ascending = 1,
        [EnumMember]
        Descending = 2
    }
    [DataContract(Name = "APIRequestBase", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public abstract class APIRequestBase
    {
        [DataMember]
        public virtual string LogonName { get; set; }
        [DataMember]
        public virtual string CorrelationId { get; set; }
        [DataMember]
        public virtual string ClientServerName { get; set; }
        [DataMember]
        public virtual string ConsumerCode { get; set; }

    }
    [DataContract(Name = "ServiceAPIRequestBase", Namespace = "http://SBS.Services.Internal.DataContracts")]
    public class ServiceAPIRequestBase
    {
        [DataMember]
        public virtual Uri ServiceUrl { get; set; }
        [DataMember]
        public virtual string APIUserName { get; set; }
        [DataMember]
        public virtual string APIPassword { get; set; }
    }
    public static class APIResources
    {
        public const string MessageDelivery = "api/messageDelivery/addMessageDelivery";
        public const string SecuritySignIn = "api/authentication/signin";
        public const string SecuritySignOut = "api/authentication/signout";
        public const string SecurityAbility = "api/ability/getAbilities";
    }
}
