using System.Runtime.Serialization;

namespace Domain.Model.Enum
{
    public enum OrderStatus
    {
        [EnumMember(Value = "CREATED")]
        CREATED,

        [EnumMember(Value = "SHIPPED")]
        SHIPPED,

        [EnumMember(Value = "ACCEPTED")]
        ACCEPTED,

        [EnumMember(Value = "INTEGRATESUCCESS")]
        INTEGRATESUCCESS,

        [EnumMember(Value = "INTEGRATEFAILURE")]
        INTEGRATEFAILURE,

        [EnumMember(Value = "CONTROLESUCCESS")]
        CONTROLESUCCESS,

        [EnumMember(Value = "CONTROLFAILURE")]
        CONTROLFAILURE,

        [EnumMember(Value = "PAYMENTSUCCESS")]
        PAYMENTSUCCESS,

        [EnumMember(Value = "PAYMENTFAILURE")]
        PAYMENTFAILURE,
    }
}