using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Constants
{
    public static class Constant
    {
    }
    public enum PaymentMethod
    {
        COD = 1,
        CREDIT_CARD = 2,
        M_BANKING = 3
    }
    public enum PaymentStatus
    {
        WAITING = 1,
        PURCHASED = 2
    }
    public enum OrderStatus
    {
        PREPARE = 1,
        SHIPER_RECEIVED = 2,
        SHIPPING = 3,
        SHIPPED = 4
    }
}
