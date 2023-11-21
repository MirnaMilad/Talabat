using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.core.Entities.OrderAggrigation
{
    
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Received")]
        PaymentRecevied,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
